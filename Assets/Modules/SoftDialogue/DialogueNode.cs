using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {

    public abstract class DialogueNode {

        public virtual void Visit(DialogueContext context) {
            DoEffects(context);
            Display(context);
        }
        protected void DoEffects(DialogueContext context) {
            context.DoEffects(effects);
        }
        protected virtual void Display(DialogueContext context) { }
        public virtual DialogueNode Next(DialogueContext context) { return null; }

        protected List<DialogueEffect> effects = new List<DialogueEffect>(); // TODO : create class for 
        public long nodeId; // for jumps
        public bool canSkip = true; // can't skip a dialog for instance
        public bool autoSkip = false; // some stuff don't have to be skiped by the player (transition...)
        public bool wasVisited = false; // for skips

        protected DialogueEffect lastEffect = null;

        public DialogueNode AddEffect(DialogueEffect effect = null) {
            if (effect == null)
                effect = new DialogueEffect();
            effects.Add(effect);
            lastEffect = effect;
            return this;
        }

        public DialogueNode AddCondition(string condition) {
            if (lastEffect == null)
                throw new Exception("Error while adding condition: no effect added yet.");
            if (!(lastEffect is DialogueConditionalEffect))
                throw new Exception("Error while adding condition: effect is not conditional.");
            ((DialogueConditionalEffect)lastEffect).AddCondition(condition);
            return this;
        }

        public DialogueNode AddHandler(DialogueEffectHandler handler) {
            if (lastEffect == null)
                throw new Exception("Error while adding handler: no effect added yet!");
            lastEffect.AddHandler(handler);
            return this;
        }
    }

    public class DialogueSpeech : DialogueNode {

        public DialogueSpeech(string text = "", string name = "", DialogueEffect[] effects = null) : base() {
            this.text = text;
            this.name = name;
            if (effects != null) {
                foreach (DialogueEffect effect in effects) {
                    AddEffect(effect);
                    lastEffect = effect;
                }
            }
        }

        protected override void Display(DialogueContext context) {
            context.SetName(name);
            context.SetText(text);
        }

        private string name;
        private string text;
    }

    public class DialogueFork : DialogueNode {

        public DialogueFork(long nodeId, DialogueEffect[] effects = null) : base() {
            this.nodeId = nodeId;
            if (effects != null) {
                foreach (DialogueEffect effect in effects) {
                    AddEffect(effect);
                    lastEffect = effect;
                }
            }
        }

    }

    public class DialogueActiveChoice : DialogueFork {

        public DialogueActiveChoice(long nodeId, DialogueActiveOption[] options, DialogueEffect[] effects = null) : base(nodeId, effects) {
            this.canSkip = false;
            if (options != null)
                foreach (DialogueActiveOption option in options)
                    this.options.Add(option);
        }

        protected override void Display(DialogueContext context) {
            if (options.Count == 0)
                throw new System.Exception("Error while displaying DialogActiveChoice : options can't be empty.");
            context.ShowChoice(options);
        }

        public DialogueActiveChoice AddOption(DialogueActiveOption option) {
            options.Add(option);
            return this;
        }

        List<DialogueActiveOption> options = new List<DialogueActiveOption>();
    }

    public class DialoguePassiveChoice : DialogueFork {
        public DialoguePassiveChoice(long nodeId, DialoguePassiveOption[] options = null, DialogueEffect[] effects = null) : base(nodeId, effects) {
            this.autoSkip = true;
            if (options != null)
                foreach (DialoguePassiveOption option in options)
                    this.options.Add(option);
        }

        public override void Visit(DialogueContext context) {
            if (options.Count == 0)
                throw new System.Exception("Error while visiting DialoguePassiveChoice : options can't be empty.");
            foreach (DialoguePassiveOption option in options) {
                if (option.AreConditionsVerified(context)) {
                    nextNodeId = option.nextNodeId;
                    break;
                }
            }
            if (nextNodeId == -1)
                throw new System.Exception("Error while checking DialogPassiveChoice : no outcome.");
            DoEffects(context);
        }

        public override DialogueNode Next(DialogueContext context) {
            return context.GetDialogueNode(nextNodeId);
        }

        public DialoguePassiveChoice AddOption(DialoguePassiveOption option) {
            options.Add(option);
            return this;
        }

        List<DialoguePassiveOption> options = new List<DialoguePassiveOption>();

        private long nextNodeId = -1;
    }

    public class DialogueBranch : DialogueNode {
        public DialogueBranch(long nodeId, long nextNodeId, DialogueEffect[] effects = null) : base() {
            this.nodeId = nodeId;
            this.nextNodeId = nextNodeId;
            if (effects != null) {
                foreach (DialogueEffect effect in effects) {
                    AddEffect(effect);
                    lastEffect = effect;
                }
            }
        }

        public DialogueBranch AddNode(DialogueNode node) {
            nodes.Add(node);
            return this;
        }

        public override void Visit(DialogueContext context) {
            if (currentNodeIndex == 0)
                DoEffects(context);
            if (currentNodeIndex < nodes.Count)
                currentNode.Visit(context);
        }

        public override DialogueNode Next(DialogueContext context) {
            currentNodeIndex++;
            if (currentNodeIndex == nodes.Count) {
                currentNodeIndex = 0;
                return context.GetDialogueNode(nextNodeId);
            }
            return this;
        }

        public List<DialogueNode> nodes = new List<DialogueNode>();
        public int currentNodeIndex = 0;
        protected DialogueNode currentNode {
            get { return nodes[currentNodeIndex]; }
        }

        public long nextNodeId;
    }

    public abstract class DialogueInteractiveNode : DialogueNode {

        public DialogueInteractiveNode(long nodeId, DialogueEffect[] effects = null) : base() {
            this.nodeId = nodeId;
            if (effects != null) {
                foreach (DialogueEffect effect in effects) {
                    AddEffect(effect);
                    lastEffect = effect;
                }
            }
        }

        public override void Visit(DialogueContext context) {
            BeginInteractiveRoutine(context);
        }

        protected virtual void BeginInteractiveRoutine(DialogueContext context) {
            this.context = context;
            // trigger the effects of the node once
            // BEWARE! if there are waits in those, behaviour isn't defined!
            DoEffects(context);
            // node takes over!
            context.VisitNode(null);
        }

        protected void EndInteractiveRoutine(long nextNodeId) {
            context.Active(context.GetDialogueNode(nextNodeId));
        }

        protected void EndInteractiveRoutine(DialogueNode nextNode) {
            context.Active(nextNode);
        }

        #region INTERACTIVE_DIALOGUE_FUNCTIONS

        // use this if you want to use the dialog engine during the interactive routine
        protected void InteractiveDialogue(DialogueNode dialogueNode, Action callback = null) {
            context.StartCoroutine(DoInteractiveDialogue(dialogueNode, callback));
        }
        protected IEnumerator DoInteractiveDialogue(DialogueNode dialogueNode, Action callback = null) {
            context.Active(dialogueNode);
            yield return new WaitWhile(delegate { return context.IsActive(); });
            if (callback != null)
                callback();
        }

        protected void InteractiveDialogueQueue(DialogueNode[] dialogueNodes, Action callback = null) {
            context.StartCoroutine(DoInteractiveDialogueQueue(dialogueNodes, callback));
        }
        protected IEnumerator DoInteractiveDialogueQueue(DialogueNode[] dialogueNodes, Action callback = null) {
            foreach (DialogueNode dialogueNode in dialogueNodes) {
                context.Active(dialogueNode);
                yield return new WaitWhile(delegate { return context.IsActive(); });
            }
            if (callback != null)
                callback();
        }

        #endregion

        protected DialogueContext context;
    }

}
