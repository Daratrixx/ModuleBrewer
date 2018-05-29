using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {
    public abstract class DialogueOption {
        public DialogueOption(long nextNodeId) {
            this.nextNodeId = nextNodeId;
        }

        public long nextNodeId;
    }

    public class DialogueActiveOption : DialogueOption {
        public DialogueActiveOption(long nextNodeId, string optionName) : base(nextNodeId) {
            this.optionName = optionName;
        }
        public string optionName;
    }

    public class DialoguePassiveOption : DialogueOption {
        public DialoguePassiveOption(long nextNodeId, string[] conditions) : base(nextNodeId) {
            if (conditions == null)
                throw new System.Exception("Error while initialising DialoguePassiveOption : conditions can't be null.");
            this.conditions = conditions;
        }

        public bool AreConditionsVerified(DialogueContext context) {
            foreach (string condition in conditions)
                if (!context.GetCondition(condition))
                    return false;
            return true;
        }

        private string[] conditions;
    }

}
