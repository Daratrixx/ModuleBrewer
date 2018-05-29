using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoftDialogue {

    public class DialogueContext : MonoBehaviour {

        public GameObject namePanel;
        public Text nameText;
        public GameObject textPanel;
        public Text textText;
        public GameObject choicePanel;
        public Transform choiceLayout;
        public GameObject choiceButtonPrefab;

        public DialogueAudioManager audioManager;
        public new Camera camera;
        public Transform spriteHolder;

        private DialogueNode currentNode = null;
        private bool isActive = false;
        private bool isVisitingNode = false;
        private bool mustEndVisit = false;
        private bool isWaiting = false;
        private float waitingDuration = 0;

        public bool IsVisitingNode() { return isVisitingNode; }
        public bool IsActive() { return isActive; }

        private Dictionary<long, DialogueNode> dialogueNodes = new Dictionary<long, DialogueNode>();
        private Dictionary<long, System.Object> contextObjects = new Dictionary<long, System.Object>();
        private Dictionary<long, DialogueSprite> contextSpriteRenderers = new Dictionary<long, DialogueSprite>();
        private Dictionary<long, AudioClip> contextAudioClips = new Dictionary<long, AudioClip>();

        private HashSet<DialogueSprite> animatingSprites = new HashSet<DialogueSprite>();

        private HashSet<string> contextConditions = new HashSet<string>();

        public void Active(DialogueNode node) {
            if (!isActive) {
                if (node == null) {
                    Debug.LogWarning("Trying to active context with a null Node. Does nothing.");
                } else {
                    isActive = true;
                    VisitNode(node);
                }
            } else {
                Debug.LogWarning("Trying to active context while it was already active. Does nothing.");
            }
        }

        public void VisitNode(DialogueNode node) {
            currentNode = node;
            if (currentNode != null) {
                currentNode.Visit(this);
            } else {
                Debug.Log("Trying to visit a null Node. Turning off SoftDialogue.");
                SetName("");
                SetText("");
                HideChoice();
                isActive = false;
            }
        }

        public void DoEffects(List<DialogueEffect> effects) {
            StartCoroutine(DoEffectsCoroutine(effects));
        }

        private IEnumerator DoEffectsCoroutine(List<DialogueEffect> effects) {
            isVisitingNode = true;
            if (effects != null) {
                foreach (DialogueEffect effect in effects) {
                    while (effect.DoEffect(this)) {
                        if (!mustEndVisit) {
                            if (isWaiting) {
                                yield return new WaitForSeconds(waitingDuration);
                                isWaiting = false;
                            }
                        }
                    }
                }
            }
            mustEndVisit = false;
            isVisitingNode = false;
            if (currentNode != null && currentNode.autoSkip) {
                Debug.Log("autoSkip");
                NextNode();
            }
        }

        public void EndNodeVisit() {
            if (currentNode != null && currentNode.canSkip && isVisitingNode && !mustEndVisit) {
                mustEndVisit = true;
            }
        }

        public void NextNode() {
            if (currentNode != null && currentNode.canSkip) {
                VisitNode(currentNode.Next(this));
            }
        }

        #region INPUT_FUNCTIONS

        public void InputNext() {
            if (isVisitingNode) {
                EndNodeVisit();
            } else {
                NextNode();
            }
        }

        public void InputToggleSkip() {

        }
        public void InputToggleAuto() {

        }

        #endregion

        #region INTERFACE_CONTROL_FUNCTIONS

        public void SetName(string name = "") {
            if (name != "") {
                namePanel.SetActive(true);
                nameText.text = name;
            } else {
                namePanel.SetActive(false);
                nameText.text = "";
            }
        }

        public void SetText(string text = "") {
            if (text != "") {
                textPanel.SetActive(true);
                textText.text = text;
            } else {
                textPanel.SetActive(false);
                textText.text = "";
            }
        }

        public void ShowChoice(ICollection<DialogueActiveOption> options) {
            //isVisitingNode = true;
            ClearChoice();
            choicePanel.SetActive(true);
            foreach (DialogueActiveOption option in options) {
                GameObject choiceButton = GameObject.Instantiate(choiceButtonPrefab, choiceLayout);
                choiceButton.GetComponent<DialogueChoiceButton>().Init(option.nextNodeId, option.optionName, this);
            }
        }
        public void ClearChoice() {
            Transform[] children = new Transform[choiceLayout.childCount];
            for (int i = 0; i < choiceLayout.childCount; i++)
                children[i] = choiceLayout.GetChild(i);
            foreach (Transform t in children)
                Destroy(t.gameObject);
            choiceLayout.DetachChildren();
        }
        public void HideChoice() {
            choicePanel.SetActive(false);
            ClearChoice();
            //isVisitingNode = false;
        }

        #endregion

        #region CONTEXT_TIME

        public void Wait(float duration) {
            isWaiting = true;
            waitingDuration = duration;
        }

        #endregion

        #region CONTEXT_CONDITIONS_FUNCTIONS

        public void SetCondition(string condition) {
            if (contextConditions.Contains(condition))
                throw new Exception("Error while setting condition : " + condition + " was already defined and wasn't freed.");
            contextConditions.Add(condition);
        }
        public void FreeCondition(string condition) {
            if (!contextConditions.Contains(condition))
                throw new Exception("Error while freeing condition : " + condition + " is undefined.");
            contextConditions.Remove(condition);
        }
        public bool GetCondition(string condition) {
            if (condition[0] == '!') {
                return !contextConditions.Contains(condition.Substring(1));
            }
            return contextConditions.Contains(condition);
        }

        #endregion

        #region CONTEXT_OBECTS_FUNCTIONS

        public void SetContextObject(long id, System.Object o) {
            if (contextObjects.ContainsKey(id))
                throw new System.Exception("Error while setting context object : id " + id + " was already defined and wasn't freed.");
            contextObjects[id] = o;
            if (o is DialogueSprite) {
                DialogueSprite ds = contextSpriteRenderers[id] = (DialogueSprite)o;
                GameObject go = new GameObject("Sprite(" + id + ")");
                go.transform.parent = spriteHolder;
                ds.spriteRenderer = go.AddComponent<SpriteRenderer>();
                ds.SetVisible(false);
            } else if (o is AudioClip) {
                contextAudioClips[id] = (AudioClip)o;
            }
        }
        public void FreeContextObject(long id) {
            if (!contextObjects.ContainsKey(id))
                throw new System.Exception("Error while freeing context object : id " + id + " is undefined.");
            System.Object o = contextObjects[id];
            if (o is SpriteRenderer) {
                contextSpriteRenderers[id].Free();
                contextSpriteRenderers.Remove(id);
            } else if (o is AudioClip) {
                contextAudioClips.Remove(id);
            }
            contextObjects.Remove(id);
        }
        public System.Object GetContextObject(long id) {
            if (!contextObjects.ContainsKey(id))
                throw new System.Exception("Error while getting context object : id " + id + " is undefined.");
            return contextObjects[id];
        }
        public T GetContextObject<T>(long id) {
            if (!contextObjects.ContainsKey(id))
                throw new System.Exception("Error while getting context object : id " + id + " is undefined.");
            return (T)contextObjects[id];
        }

        #endregion

        #region SPRITES_CONTROL_FUNCTIONS

        public void SetSpriteSource(long objectId, string assetPath) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while setting sprite source : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].SetSource(assetPath);
        }

        public void SetSpriteVisible(long objectId, bool visible) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while setting sprite visibility : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].SetVisible(visible);
        }

        public void SetSpriteSize(long objectId, Vector2 size) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while setting sprite size : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].SetSize(size);
        }

        public void SetSpritePosition(long objectId, Vector3 position) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while setting sprite position : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].SetPosition(position);
        }

        public void SnapSpriteToPosition(long objectId, Vector3 position, SpriteSnapPosition snapPosition = SpriteSnapPosition.Bottom) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while snaping sprite to position : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].SnapToPosition(position, snapPosition);
        }

        public void FlipSprite(long objectId, bool flipX, bool flipY) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while fliping sprite : objectId " + objectId + " is undefined.");
            contextSpriteRenderers[objectId].Flip(flipX, flipY);
        }

        public void ScaleSprite(long objectId, float scaleDuration, Vector2 targetSize) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while scaling sprite : objectId " + objectId + " is undefined.");
            DialogueSprite ds = contextSpriteRenderers[objectId];
            if (!ds.isAnimating)
                animatingSprites.Add(ds);
            ds.Scale(scaleDuration, targetSize);
        }

        public void MoveSprite(long objectId, float moveDuration, Vector3 targetPosition, SpriteSnapPosition targetSnapPosition) {
            if (!contextSpriteRenderers.ContainsKey(objectId))
                throw new System.Exception("Error while moving sprite : objectId " + objectId + " is undefined.");
            DialogueSprite ds = contextSpriteRenderers[objectId];
            if (!ds.isAnimating)
                animatingSprites.Add(ds);
            ds.Move(moveDuration, targetPosition, targetSnapPosition);
        }

        #endregion

        #region AUDIO_CONTROL_FUNCTIONS

        public void SetAudioSource(long objectId, string assetPath) {
            if (!contextAudioClips.ContainsKey(objectId))
                throw new System.Exception("Error while setting audio source : objectId " + objectId + " is undefined.");
            contextAudioClips[objectId] = Resources.Load<AudioClip>(assetPath);
        }

        public void PlayAudio(long objectId) {
            if (!contextAudioClips.ContainsKey(objectId))
                throw new System.Exception("Error while playing audio : objectId " + objectId + " is undefined.");
            audioManager.PlayAudio(contextAudioClips[objectId]);
        }

        public void PlayMusic(long objectId) {
            if (!contextAudioClips.ContainsKey(objectId))
                throw new System.Exception("Error while playing audio : objectId " + objectId + " is undefined.");
            audioManager.PlayMusic(contextAudioClips[objectId]);
        }

        public void StopAudio() {
            audioManager.StopAudio();
        }

        public void StopMusic() {
            audioManager.StopMusic();
        }

        #endregion

        #region CREATION_HELP_FUNCTIONS

        public DialogueNode GetDialogueNode(long nodeId) {
            if (nodeId == -1)
                Debug.Log("End of content reached.");
            else if (!dialogueNodes.ContainsKey(nodeId))
                throw new System.Exception("Error while getting DialogueNode : nodeId " + nodeId + " does not exist!");
            else
                return dialogueNodes[nodeId];
            return null;
        }

        public void RegisterDialogueNode(long nodeId, DialogueNode node) {
            if (dialogueNodes.ContainsKey(nodeId))
                throw new System.Exception("Error while regeistering dialogue node : nodeId " + nodeId + " was already registered!");
            dialogueNodes[nodeId] = node;
        }

        public DialogueActiveChoice CreateDialogueActiveChoice(long nodeId, DialogueActiveOption[] options = null, DialogueEffect[] effects = null) {
            DialogueActiveChoice output = new DialogueActiveChoice(nodeId, options, effects);
            if (dialogueNodes.ContainsKey(nodeId))
                throw new System.Exception("Error while regeistering CreateDialogueActiveChoice : nodeId " + nodeId + " was already registered!");
            dialogueNodes[nodeId] = output;
            return output;
        }

        public DialoguePassiveChoice CreateDialoguePassiveChoice(long nodeId, DialoguePassiveOption[] options = null, DialogueEffect[] effects = null) {
            DialoguePassiveChoice output = new DialoguePassiveChoice(nodeId, options, effects);
            if (dialogueNodes.ContainsKey(nodeId))
                throw new System.Exception("Error while regeistering DialoguePassiveChoice : nodeId " + nodeId + " was already registered!");
            dialogueNodes[nodeId] = output;
            return output;
        }

        public DialogueBranch CreateDialogueBranch(long nodeId, long nextNodeId, DialogueEffect[] effects = null) {
            DialogueBranch output = new DialogueBranch(nodeId, nextNodeId, effects);
            if (dialogueNodes.ContainsKey(nodeId))
                throw new System.Exception("Error while regeistering DialogueBranch : nodeId " + nodeId + " was already registered!");
            dialogueNodes[nodeId] = output;
            return output;
        }

        #endregion

        #region UNITY_FUNCTIONS

        void Update() {
            if (isActive) {
                if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    InputNext();
            }
            // sprite animation stuff
            float deltaTime = Time.deltaTime;
            HashSet<DialogueSprite> endedAnimation = new HashSet<DialogueSprite>();
            foreach (DialogueSprite ds in animatingSprites) {
                if (!ds.Animate(deltaTime)) {
                    endedAnimation.Add(ds);
                }
            }
            foreach (DialogueSprite ds in endedAnimation) {
                animatingSprites.Remove(ds);
            }
        }

        #endregion

    }



}