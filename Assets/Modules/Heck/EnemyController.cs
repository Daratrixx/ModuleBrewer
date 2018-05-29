using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class EnemyController : MonoBehaviour {

        public Character character = null;
        public Transform target = null;

        public float agroRange = 10f;
        public float followRange = 40f;
        private float attackRange = 2f;
        public float movementValidationRange = 1;

        private float attackInterval = 3f;
        private float attackCooldown = 3f;

        private float behaviourUpdateDelay = .5f;
        public Action currentAction = Action.Idle;
        private AttackChoice currentAttackChoice = null;

        private Vector3 startingPoint;
        private Vector3 movementDestinationPoint;

        public AttackChoice[] attackChoices;
        public Vector3[] roamPoints;
        private int currentRoamPoint = 0;
        public bool waitAtRoamPoint;

        private void Start() {
            startingPoint = character.groundPosition;
            StartCoroutine(DoUpdateBehavior());
        }

        protected virtual IEnumerator DoUpdateBehavior() {
            while (character != null && character.isAlive) {
                PickTarget();
                ChooseAction();
                switch (currentAction) {
                    case Action.Roam:
                        if (Vector3.Distance(character.transform.position, roamPoints[currentRoamPoint]) <= movementValidationRange) {
                            currentRoamPoint = (currentRoamPoint + 1) % roamPoints.Length;
                            if (waitAtRoamPoint)
                                currentAction = Action.Idle;
                            else
                                movementDestinationPoint = roamPoints[currentRoamPoint];
                        }
                        break;
                    case Action.Attack:
                        ChooseAttack();
                        movementDestinationPoint = target.position;
                        break;
                    case Action.Flank:
                        if (Vector3.Distance(character.transform.position, movementDestinationPoint) <= movementValidationRange) {
                            currentAction = Action.Attack;
                            ChooseAttack();
                            movementDestinationPoint = target.position;
                        }
                        break;
                    default:
                        movementDestinationPoint = startingPoint;
                        break;
                }
                yield return new WaitForSeconds(behaviourUpdateDelay);
            }
        }

        private void Update() {
            if (character == null) return;
            if (!character.isAlive) { character = null; return; }

            Movement(movementDestinationPoint);
            if (target != null && currentAction == Action.Attack) {
                Attack();
            }
            if (attackCooldown > 0)
                attackCooldown -= Time.fixedDeltaTime;
        }

        protected virtual void PickTarget() {
            if (target == null || Vector3.Distance(target.position, character.transform.position) >= followRange) {
                Character c = Character.characters
                .Where(x => x.team != character.team)
                .Where(x => Vector3.Distance(x.transform.position, character.transform.position) <= agroRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, character.transform.position))
                .DefaultIfEmpty(null)
                .First();
                if (c != null)
                    target = c.transform;
                else target = null;
            }
        }

        protected virtual void ChooseAction() {
            if (target == null) {
                if (roamPoints.Length > 0 && currentAction != Action.Roam) {
                    currentAction = Action.Roam;
                    movementDestinationPoint = roamPoints[currentRoamPoint];
                } else if (roamPoints.Length == 0 && currentAction != Action.Idle) {
                    currentAction = Action.Idle;
                    movementDestinationPoint = startingPoint;
                }
            } else {
                if (currentAction != Action.Attack && currentAction != Action.Flank) {
                    if (Random.Range(.0f, 1.0f) <= 0.5f) {
                        currentAction = Action.Flank;
                        movementDestinationPoint = target.position + Vector3.Cross((character.transform.position - target.position).normalized, Vector3.up) * 3;
                    } else {
                        currentAction = Action.Attack;
                    }
                }
            }
        }

        protected virtual void ChooseAttack() {
            if (target == null) { currentAttackChoice = null; return; }
            if (currentAttackChoice != null) { return; }
            float distance = Vector3.Distance(character.transform.position, target.transform.position); ;
            float angle = Vector3.Dot(target.transform.position - character.transform.position, character.forward); ;
            foreach (AttackChoice ac in attackChoices) {
                if (ac.minRange < distance && ac.maxRange > distance
                    && ac.minAngle < angle && ac.maxAngle > angle) {
                    currentAttackChoice = ac;
                }
            }
            currentAttackChoice = null;
        }

        private float ditanceToTarget;
        private bool IsInRange(Transform target) {
            ditanceToTarget = Vector3.Distance(character.transform.position, target.position);
            return ditanceToTarget < agroRange;
        }

        private void Movement(Vector3 targetPosition) {
            if (Vector3.Distance(character.transform.position, targetPosition) >= movementValidationRange) {
                character.Move(Vector3.Scale(targetPosition - character.transform.position, new Vector3(1, 0, 1)).normalized);
            } else {
                character.Rotate(Vector3.Scale(targetPosition - character.transform.position, new Vector3(1, 0, 1)).normalized);
                character.NoMove();
            }
        }

        private void Attack() {
            if (IsInRange(target) && attackCooldown <= 0) {
                attackCooldown = attackInterval;
                character.Bash();
                currentAction = Action.Idle;
                currentAttackChoice = null;
            }
        }


        private void OnDrawGizmos() {
            if (character == null || !character.isAlive)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(startingPoint, 0.2f);
            Gizmos.DrawLine(character.transform.position, startingPoint);
            if (roamPoints != null && roamPoints.Length > 0) {
                Gizmos.color = Color.cyan;
                foreach (Vector3 p in roamPoints) {
                    Gizmos.DrawSphere(p, 0.2f);
                }
                Gizmos.DrawLine(character.transform.position, roamPoints[currentRoamPoint]);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(movementDestinationPoint, 0.2f);
            Gizmos.DrawLine(character.transform.position, movementDestinationPoint);
            Gizmos.color = Color.white;
            if (target == null)
                Gizmos.DrawWireSphere(character.transform.position, agroRange);
            else
                Gizmos.DrawWireSphere(character.transform.position, followRange);
        }

    }

    [System.Serializable]
    public class AttackChoice {
        public float minRange, maxRange;
        public float minAngle, maxAngle;
        public float chargeRange;
        public int attack;
    }

    public enum Action {
        Idle, Roam, Guard, CircleGuard, Flank, Attack
    }

}
