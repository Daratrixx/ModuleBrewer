using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class InteractableDoor : Interactable {

        public Collider doorCollider;
        public Animator animatorController;
        public bool oneWayDoor = false; // if one way, open only from behind
        public bool lockedDoor;
        public string requiredKey;

        public bool isOpened;

        private void Start() {
            if(isOpened) {
                animatorController.CrossFade("Opened", 0.1f);
                interactionArea.enabled = false;
                doorCollider.enabled = false;
                enabled = false;
            }
        }

        public override void Interact(Character interactor) {
            if (!oneWayDoor) {
                Open(interactor);
            } else if(Dot(interactor.transform.position) < 0) {
                Open(interactor);
            }
        }

        public void Open(Character interactor) {
            StartCoroutine(DoOpen(interactor));
        }

        private IEnumerator DoOpen(Character interactor) {
            animatorController.CrossFade("Open", 0.1f);
            yield return new WaitForSeconds(interactionDuration);
            interactionArea.enabled = false;
            doorCollider.enabled = false;
            enabled = false;
            isOpened = true;
        }

        public bool InOpeningArea(Vector3 position) {
            return true;
        }
    }
}
