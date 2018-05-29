using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class Character : MonoBehaviour, Attackable {

        public static List<Character> characters = new List<Character>();

        public int team = 0;

        public ItemInstance[] initialItems = new ItemInstance[0];
        public Inventory inventory = new Inventory();
        public Spell[] spells = new Spell[0];
        public void UseSpell(int idSpell, Character target) {
            if (idSpell < spells.Length)
                StartCoroutine(spells[idSpell].OnTriggerEffects(this, target));
        }
        public void AddBuff(BuffInstance instance) {
            buffs.Add(instance);
            StartCoroutine(instance.buff.OnStart(instance));
        }
        public void RemoveBuff(BuffInstance instance) {
            if (!instance.removed) {
                buffs.Remove(instance);
                instance.removed = true;
            }
        }
        public List<BuffInstance> buffs = new List<BuffInstance>();

        public Animator animatorController;
        public PlayerController controller;
        public AudioSource audioSource;

        public float rotationSpeed = 7.5f;
        public float movementSpeed = 10;

        public CharacterMove dash;

        public MoveSet currentMoveSet;
        public Weapon weapon;

        private CharacterMove currentFightMove;
        private float elapsedAnimationTime;
        private int currentAnimationFrame;

        public Vector3 groundPosition {
            get {
                RaycastHit info;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out info, gravityCollisionLayer))
                    return info.point;
                return transform.position;
            }
        }

        void Start() {
            if (audioSource == null) {
                audioSource = GetComponent<AudioSource>();
            }
            if (audioSource == null) {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            characters.Add(this);
            foreach (ItemInstance ii in initialItems)
                inventory.AddItem(ii.item, ii.stackCount);
        }

        public Vector3 forward {
            get { return transform.forward; }
            set { transform.forward = value; }
        }
        public Vector3 right {
            get { return transform.right; }
        }
        public Vector3 up {
            get { return transform.up; }
        }

        private bool canStartAttack {
            get { return !isFightMoving; }
        }
        private bool canWalk {
            get { return !isFightMoving; }
        }
        private bool canRotate {
            get { return currentFightMove == null || currentFightMove.allowsRotation; }
        }

        public int currentHealth = 500, maxHealth = 500;
        public int currentEnergy = 150, maxEnergy = 150;

        public bool CanDamage() {
            return !isEvading;
        }
        public bool isAlive {
            get { return currentHealth > 0; }
        }
        public bool isEvading {
            get { return evasionFrame; }
        }
        public bool isVulnerable {
            get { return vulnerabilityFrame; }
        }


        public void Heal(int heal) {
            if (heal > 0) currentHealth += heal;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

        public void Damage(int damage) {
            if (damage > 0)
                currentHealth -= damage;
            if (!isAlive)
                Die();
        }
        public void Damage(float amount, DamageType type) {
            if (amount > 0)
                currentHealth -= (int)amount;
            if (!isAlive)
                Die();
        }

        public void Die() {
            if (isFightMoving)
                StopFightMove();
            StartCoroutine(DoDie());
        }

        private IEnumerator DoDie() {
            currentHealth = 0;
            float decay = 1;
            while (decay > 0) {
                transform.localScale = new Vector3(decay, decay, decay);
                decay -= 0.025f;
                yield return null;
            }
            Character.characters.Remove(this);
            Destroy(gameObject);
        }

        public Attackable GetAttackableRoot() {
            return this;
        }

        private bool isAttacking;

        private InputCommand inputBuffer = InputCommand.None;
        private Coroutine inputBufferCleanRoutine;
        public const float inputBufferDuration = 0.5f;
        private Vector3 inputBufferDirection;

        private IEnumerator DoSetInputBuffer(InputCommand inputBuffer) {
            this.inputBuffer = inputBuffer;
            yield return new WaitForSeconds(inputBufferDuration);
            this.inputBuffer = InputCommand.None;
            inputBufferCleanRoutine = null;
        }

        public bool Attack(bool movementinput = false) {
            if (canStartAttack && !ChainMove(InputCommand.Attack)) {
                StartFightMove(currentMoveSet.attackMoves[0], true);
                return true;
            }
            if (inputBufferCleanRoutine != null)
                StopCoroutine(inputBufferCleanRoutine);
            inputBufferCleanRoutine = StartCoroutine(DoSetInputBuffer(InputCommand.Attack));
            return false;
        }

        public bool Bash(bool movementinput = false) {
            if (canStartAttack && !ChainMove(InputCommand.Bash)) {
                StartFightMove(currentMoveSet.bashMoves[0], true);
                return true;
            }
            if (inputBufferCleanRoutine != null)
                StopCoroutine(inputBufferCleanRoutine);
            inputBufferCleanRoutine = StartCoroutine(DoSetInputBuffer(InputCommand.Bash));
            return false;
        }

        public bool Dash() {
            if (dash != null && (!isFightMoving || currentFightMove.isCancelable)) {
                StartFightMove(dash);
                return true;
            }
            if (inputBufferCleanRoutine != null)
                StopCoroutine(inputBufferCleanRoutine);
            inputBufferCleanRoutine = StartCoroutine(DoSetInputBuffer(InputCommand.Dash));
            inputBufferDirection = forward;
            return false;
        }
        public bool Dash(Vector3 direction) {
            if (dash != null && (!isFightMoving || currentFightMove.isCancelable)) {
                forward = direction.normalized;
                StartFightMove(dash);
                return true;
            }
            if (inputBufferCleanRoutine != null)
                StopCoroutine(inputBufferCleanRoutine);
            inputBufferCleanRoutine = StartCoroutine(DoSetInputBuffer(InputCommand.Dash));
            inputBufferDirection = direction;
            return false;
        }

        public bool ChainMove(InputCommand command) {
            if (isChaining) {
                for (int i = 0; i < currentFightMove.chainMoves.Length; ++i) {
                    if (currentFightMove.chainMoves[i].command == command) {
                        StartFightMove(currentFightMove.chainMoves[i].move);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isMoving;
        public void Move(Vector3 direction) {
            if (canWalk) {
                if (!isMoving) {
                    isMoving = true;
                    CrossFadeAnimation("MoveForward", 0.3f);
                    if (isChaining)
                        StopChaining();
                }
                transform.position += direction * movementSpeed * Time.deltaTime;
            }
            if (canRotate && !damageFrame) {
                direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
                direction.y = 0;
                forward = direction.normalized;
            }
        }
        public void Strafe(Vector3 aim, Vector3 movement) {
            if (canWalk) {
                if (!isMoving) {
                    isMoving = true;
                    CrossFadeAnimation("MoveForward", 0.3f);
                    if (isChaining)
                        StopChaining();
                }
                transform.position += transform.rotation * movement * movementSpeed * Time.deltaTime;
            }
            if (canRotate && !damageFrame) {
                aim = Vector3.Slerp(forward, aim, rotationSpeed * Time.deltaTime);
                aim.y = 0;
                forward = aim.normalized;
            }
        }
        public void Rotate(Vector3 direction) {
            if (canRotate && !damageFrame) {
                direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
                direction.y = 0;
                forward = direction.normalized;
            }
            NoMove();
        }
        public void NoMove() {
            if (isMoving) {
                isMoving = false;
                BackToStandAnimation();
            }
        }


        #region ANIMATION_CONTROLLS

        private void StartAnimation(string animationName) {
            //animatorController.CrossFade(animationName, 0.1f);
            animatorController.Play(animationName, -1, 0);
            elapsedAnimationTime = 0;
            currentAnimationFrame = 0;
        }

        private void CrossFadeAnimation(string animationName, float transitionDuration) {
            animatorController.CrossFade(animationName, transitionDuration);
            elapsedAnimationTime = 0;
            currentAnimationFrame = 0;
        }

        private void BackToStandAnimation() {
            StartAnimation("Stand");
        }

        #endregion

        #region MOVE_ANIMATION_CONTROLLS

        private Coroutine runFightMoveCoroutine = null;
        private bool isFightMoving;
        private Coroutine endChainingCoroutine = null;
        private bool isChaining;

        private void StartFightMove(CharacterMove move, bool isAttackMove = false) {
            currentEnergy -= move.energyCost;
            isMoving = false;
            if (isFightMoving)
                StopFightMove();
            else if (isChaining)
                StopChaining();
            isFightMoving = true;
            isAttacking = isAttackMove;
            currentFightMove = move;
            StartAnimation(move.animationName);
            runFightMoveCoroutine = StartCoroutine(RunFightMove(move));
        }

        public void PlayAudioClip(AudioClip clip) {
            audioSource.PlayOneShot(clip);
        }

        private HashSet<Coroutine> cancelableInteractionCoroutine = new HashSet<Coroutine>();

        private IEnumerator RunFightMove(CharacterMove move) {
            foreach (MoveInteraction interaction in move.GetInteractions()) {
                if (interaction.CanCancel()) {
                    cancelableInteractionCoroutine.Add(StartCoroutine(interaction.DoInteraction(this, move)));
                } else {
                    StartCoroutine(interaction.DoInteraction(this, move));
                }
            }
            yield return new WaitForSeconds(move.animationDuration);
            Debug.Log("RunFightMove end after " + move.animationDuration + " seconds.");
            cancelableInteractionCoroutine.Clear();
            isFightMoving = false;
            isAttacking = false;
            if (currentFightMove.chainMoves.Length > 0) {
                isChaining = true;
                endChainingCoroutine = StartCoroutine(RunChainingFightMove(0.3f));
            } else {
                currentFightMove = null;
                BackToStandAnimation();
            }
            // use buffered command
            switch (inputBuffer) {
                case InputCommand.Attack:
                    Attack();
                    break;
                case InputCommand.Bash:
                    Bash();
                    break;
                case InputCommand.Dash:
                    Dash(inputBufferDirection);
                    break;
                default:
                    break;
            }
            if (inputBuffer != InputCommand.None) {
                inputBuffer = InputCommand.None;
                StopCoroutine(inputBufferCleanRoutine);
                inputBufferCleanRoutine = null;
            }
        }

        private bool damageFrame = false;
        private float damageRatio = 1;
        public void SetDamageFrame(bool damage, float ratio = 1) {
            damageFrame = damage;
            damageRatio = ratio;
            if (!damage) {
                weaponDamagedTargets.Clear();
            }
        }
        private bool evasionFrame = false;
        public void SetEvasionFrame(bool evasion) {
            evasionFrame = evasion;
        }
        private bool vulnerabilityFrame = false;
        public void SetVulnerabilityFrame(bool vulnerability) {
            vulnerabilityFrame = vulnerability;
        }


        private HashSet<Attackable> weaponDamagedTargets = new HashSet<Attackable>();

        public void ApplyForce(Vector3 force) {
            transform.position += force;
        }
        public void ApplyDisplacement(Vector3 displacement) {
            transform.position += right * displacement.x +
                up * displacement.y +
                forward * displacement.z;
        }

        private IEnumerator RunChainingFightMove(float duration) {
            yield return new WaitForSeconds(duration);
            Debug.Log("RunChainingFightMove ended after " + duration + " seconds.");
            isChaining = false;
            currentFightMove = null;
            BackToStandAnimation();
        }

        private void StopFightMove() {
            StopCoroutine(runFightMoveCoroutine);
            foreach (Coroutine coroutine in cancelableInteractionCoroutine) {
                StopCoroutine(coroutine);
            }
            isFightMoving = false;
            damageFrame = false;
            evasionFrame = false;
            vulnerabilityFrame = false;
            if (isChaining) {
                StopCoroutine(endChainingCoroutine);
                endChainingCoroutine = null;
                isChaining = false;
            }
            isAttacking = false;
            currentFightMove = null;
        }
        private void StopChaining() {
            StopCoroutine(endChainingCoroutine);
            endChainingCoroutine = null;
            isChaining = false;
            currentFightMove = null;
        }

        #endregion

        private float gravitySpeed = 15;
        public const float gravityOffset = 0.05f;
        public const int gravityCollisionLayer = 1 << 10;
        private float fallDistance = 0;
        void Update() {
            float gravityEffect = Time.deltaTime * gravitySpeed;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hitInfo, gravityEffect + 1, gravityCollisionLayer)) {
                fallDistance += Vector3.Distance(transform.position, hitInfo.point);
                transform.position = hitInfo.point + Vector3.up * gravityOffset;
                if (isAlive) {
                    if (fallDistance > 20) {
                        Die();
                    } else if (fallDistance > 10) {
                        Damage((int)(maxHealth * (fallDistance - 10) / 10));
                    }
                }
                fallDistance = 0;
            } else {
                transform.position -= Vector3.up * gravityEffect;
                fallDistance += gravityEffect;
                if (isAlive && fallDistance > 25) {
                    Die();
                }
            }
        }

        void FixedUpdate() {
            if (currentEnergy < maxEnergy)
                currentEnergy += 1;
            elapsedAnimationTime += Time.fixedDeltaTime;
            currentAnimationFrame++;
            if (currentFightMove != null && !isChaining)
                ExecuteFightMove();
        }

        public void ExecuteFightMove() {
            if (isAttacking && damageFrame)
                weapon.DoDamage(damageRatio, weaponDamagedTargets);
        }
    }


    public interface Attackable {
        bool CanDamage();
        void Damage(int damage);
        void Damage(float amout, DamageType type);
        void Die();

        Attackable GetAttackableRoot();
    }

}
