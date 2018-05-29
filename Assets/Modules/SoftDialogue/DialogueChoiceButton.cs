using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoftDialogue {
    public class DialogueChoiceButton : MonoBehaviour {

        public Text buttonText;
        private long nextNodeId;
        private DialogueContext dialogueContext;

        public void SelectChoice() {
            dialogueContext.HideChoice();
            dialogueContext.VisitNode(dialogueContext.GetDialogueNode(nextNodeId));
        }

        public void Init(long nextNodeId, string optionName, DialogueContext dialogContext) {
            this.nextNodeId = nextNodeId;
            buttonText.text = optionName;
            this.dialogueContext = dialogContext;
        }
    }
}
