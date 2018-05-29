using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class InteractableCharacter : Interactable {
        
        public Character character;
        public Vector3 defaultHeadDirection;

        public string defaultAnimationName;

        public long rootDialogueId;
        public SoftDialogue.DialogueContext dialogueContext;

        private bool isTalking = false;
        private Character currentInteractor;

        public const float endDalogueDistance = 5;

        private void FixedUpdate() {
            if(isTalking) {
                if(Vector3.Distance(interactionArea.transform.position, currentInteractor.transform.position) > endDalogueDistance) {
                    EndTalk();
                }
            }
        }

        public override void Interact(Character interactor) {
            Talk(interactor);
        }

        public void Talk(Character interactor) {
            talkingCoroutine = StartCoroutine(DoTalk(interactor));
        }
        private Coroutine talkingCoroutine;
        private IEnumerator DoTalk(Character interactor) {
            currentInteractor = interactor;
            dialogueContext.Active(dialogueContext.GetDialogueNode(rootDialogueId));
            interactionArea.enabled = false;
            isTalking = true;
            yield return new WaitWhile(dialogueContext.IsActive);
            talkingCoroutine = null;
            EndTalk();
        }

        private void EndTalk() {
            currentInteractor = null;
            isTalking = false;
            interactionArea.enabled = true;
            if(talkingCoroutine != null) {
                dialogueContext.VisitNode(null);
                StopCoroutine(talkingCoroutine);
                talkingCoroutine = null;
            }
        }
    }
}
