using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Heck {

    public class PlayerController : MonoBehaviour {

        public Character character;
        public Camera playerCamera;
        public GameObject interactionPanel;
        public Text interactionText;
        public new CapsuleCollider collider;
        public MenuEquipement menuEquipement;

        public Vector2 cameraAngleLimits;
        public float cameraAngle = 20;
        public float cameraRotation = -90;
        public float cameraDistance = 4;
        public Vector3 cameraOffset = Vector3.up * 2;

        public bool gameCursor = false;
        public Character lockTarget;

        private List<Interactable> interactables = new List<Interactable>();
        private Interactable currentInteractable;

        private void Start() {
            ResetCamera();
        }

        public void SetInteractable(Interactable interactable) {
            currentInteractable = interactable;
            if (interactable != null)
                ShowInteractionDialogue(interactable.interactionText);
            else
                HideInteractionDialogue();
        }
        public void ShowInteractionDialogue(string text) {
            interactionPanel.SetActive(true);
            interactionText.text = text;
        }
        public void HideInteractionDialogue() {
            interactionPanel.SetActive(false);
        }
        public void Interact() {
            if (currentInteractable != null) {
                currentInteractable.Interact(character);
                currentInteractable = null;
            }
        }

        public void SetGameCursor(bool value) {
            if (value) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public float cameraFollowSpeed = 5;
        public void LateUpdate() {
            UpdateCamera(cameraFollowSpeed * Time.deltaTime);
        }
        private Vector3 lastAnchor;
        private Vector3 cameraAnchor {
            get {
                if (character != null) return lastAnchor = character.transform.position + cameraOffset;
                return lastAnchor;
            }
        }

        public const int cameraLayer = 1 << 10;
        public float cameraCollisionOffset = 0.5f;
        public void UpdateCamera(float deltaTime) {
            if (character.isAlive) {
                Vector3 direction;
                if (lockTarget != null)
                    direction = (lockTarget.transform.position - cameraAnchor).normalized;
                else
                    direction = new Vector3(
                        Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
                        Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
                        Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

                playerCamera.transform.forward = Vector3.Lerp(playerCamera.transform.forward, direction, deltaTime);
                RaycastHit cameraAdjutCast;
                if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
                    playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                        cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset),
                        deltaTime);
                } else {
                    playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                        cameraAnchor - direction * cameraDistance,
                        deltaTime);
                }
            } else {
                playerCamera.transform.LookAt(cameraAnchor);
            }
        }
        public void ResetCamera() {
            cameraRotation = character.transform.eulerAngles.y;
            Vector3 direction = new Vector3(Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
                Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
                Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

            playerCamera.transform.forward = direction;
            RaycastHit cameraAdjutCast;
            if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
                playerCamera.transform.position = cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset);
            } else {
                playerCamera.transform.position = cameraAnchor - direction * cameraDistance;
            }
        }

        public void FixedUpdate() {
            try {
                if (character != null && character.isAlive)
                    CheckInteractables();
            } catch (MissingReferenceException) {
                character = null;
            }
        }
        public void Update() {
            try {
                if (Menu.currentMenu == null) {
                    ControllCamera();
                    if (character != null && character.isAlive)
                        CheckInputs();
                }
            } catch (MissingReferenceException) {
                character = null;
            }
        }

        public void ControllCamera() {

            cameraAngle += UnityEngine.Input.GetAxis("Mouse Y");
            cameraRotation += UnityEngine.Input.GetAxis("Mouse X");
            cameraAngle = Mathf.Clamp(cameraAngle, cameraAngleLimits.x, cameraAngleLimits.y);
            if (cameraRotation >= 360)
                cameraRotation -= 360;
            if (cameraRotation < 0)
                cameraRotation += 360;
        }
        public void CheckInputs() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                menuEquipement.Open();
                return;
            }
            float forWalk = 0;
            float sideWalk = 0;
            bool movementInput = false;
            if (UnityEngine.Input.GetKey(KeyCode.Z) && !UnityEngine.Input.GetKey(KeyCode.S)) {
                forWalk = 1;
                movementInput = true;
            } else if (UnityEngine.Input.GetKey(KeyCode.S)) {
                forWalk = -1;
                movementInput = true;
            }
            if (UnityEngine.Input.GetKey(KeyCode.Q) && !UnityEngine.Input.GetKey(KeyCode.D)) {
                sideWalk = -1;
                movementInput = true;
            } else if (UnityEngine.Input.GetKey(KeyCode.D)) {
                sideWalk = 1;
                movementInput = true;
            }
            if (movementInput) {
                Vector3 direction = playerCamera.transform.forward * forWalk + playerCamera.transform.right * sideWalk;
                direction.y = 0;
                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                    character.Dash(direction.normalized);
                else if (UnityEngine.Input.GetMouseButtonDown(0))
                    character.Attack(true);
                else if (UnityEngine.Input.GetMouseButtonDown(1))
                    character.Bash(true);
                else if (lockTarget != null) {
                    direction.x = sideWalk;
                    direction.z = forWalk;
                    character.Strafe(playerCamera.transform.forward, direction.normalized);
                } else
                    character.Move(direction.normalized);
            } else {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                    character.Dash();
                else if (UnityEngine.Input.GetMouseButtonDown(0))
                    character.Attack(false);
                else if (UnityEngine.Input.GetMouseButtonDown(1))
                    character.Bash(false);
                else if (lockTarget != null) {
                    Vector3 direction = playerCamera.transform.forward;
                    direction.y = 0;
                    character.Rotate(direction.normalized);
                } else
                    character.NoMove();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.A)) {
                Interact();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) {
                character.UseSpell(0, lockTarget);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) {
                character.UseSpell(1, lockTarget);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) {
                character.UseSpell(2, lockTarget);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) {
                character.UseSpell(3, lockTarget);
            }
            if (UnityEngine.Input.GetMouseButtonDown(2)) {
                UpdateLockOn();
            }
        }
        public void UpdateLockOn() {
            lockTarget = GetLockOnTarget();
        }
        private Character GetLockOnTarget() {
            return Character.characters
                .Where(x => x != character && x != lockTarget && x.team != character.team)
                .Where(x => Vector3.Distance(x.transform.position, character.transform.position) < 10)
                .OrderByDescending(x => Vector3.Dot(playerCamera.transform.forward,
                    (x.transform.position - character.transform.position).normalized))
                .DefaultIfEmpty(null)
                .First();
        }
        public void CheckInteractables() {
            Collider[] colliders = Physics.OverlapBox(collider.transform.position,
                new Vector3(1, 3, 1),
                collider.transform.rotation);
            interactables.Clear();
            foreach (Collider c in colliders) {
                Interactable i = c.GetComponent<Interactable>();
                if (i != null)
                    interactables.Add(i);
            }
            interactables.Sort();
            if (interactables.Count > 0) {
                if (currentInteractable == null || !interactables.Contains(currentInteractable))
                    SetInteractable(interactables[0]);
            } else {
                SetInteractable(null);
            }
        }

        private void OnDrawGizmos() {
            if (lockTarget != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(character.transform.position, lockTarget.transform.position + new Vector3(0, 1.5f, 0));
                Gizmos.DrawSphere(lockTarget.transform.position + new Vector3(0, 1.5f, 0), 0.5f);
            }
        }

    }

}
