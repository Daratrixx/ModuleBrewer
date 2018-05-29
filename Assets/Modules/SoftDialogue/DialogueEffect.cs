using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {
    public class DialogueEffect {

        public DialogueEffect(DialogueEffectHandler[] handlers = null) {
            if (handlers != null) {
                foreach (DialogueEffectHandler handler in handlers) {
                    this.handlers.Add(handler);
                }
            }
        }

        public virtual bool DoEffect(DialogueContext context) {
            if (currentEffectIndex == handlers.Count) {
                currentEffectIndex = 0;
                return false;
            }
            handlers[currentEffectIndex++].Handle(context);
            return true;
        }
        protected int currentEffectIndex = 0;
        protected List<DialogueEffectHandler> handlers = new List<DialogueEffectHandler>();

        public void AddHandler(DialogueEffectHandler handler) {
            handlers.Add(handler);
        }
    }

    public class DialogueConditionalEffect : DialogueEffect {

        public DialogueConditionalEffect(string[] conditions = null, DialogueEffectHandler[] handlers = null) : base(handlers) {
            if(conditions != null) {
                foreach (string condition in conditions)
                    this.conditions.Add(condition);
            }
        }

        public override bool DoEffect(DialogueContext context) {
            if (currentEffectIndex == 0) {
                if (AreConditionsVerified(context))
                    return base.DoEffect(context);
            } else {
                return base.DoEffect(context);
            }
            return false;
        }

        protected bool AreConditionsVerified(DialogueContext context) {
            foreach (string condition in conditions)
                if (!context.GetCondition(condition))
                    return false;
            return true;
        }

        public DialogueConditionalEffect AddCondition(string condition) {
            conditions.Add(condition);
            return this;
        }

        protected List<string> conditions = new List<string>();
    }

}
