using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftDialogue;


public class DialogueCombatNode : DialogueInteractiveNode {

    public DialogueCombatNode(long nodeId, CombatUI combatUI, DialogueEffect[] effects = null) : base(nodeId, effects) {
        this.combatUI = combatUI;
        combatUI.combatNode = this;
    }

    protected override void BeginInteractiveRoutine(DialogueContext context) {
        base.BeginInteractiveRoutine(context);
        StartCombat();
    }

    public void StartCombat() {
        Start();
    }

    public void EndCombat(bool victory) {
        HideCombatInterface();
        if (victory) {
            Victory();
        } else {
            Defeat();
        }
    }

    public void Start() {
        InteractiveDialogue(startDialogue, delegate { ShowCombatInterface(); });
    }

    public void Pet() {
        HideCombatInterface();
        InteractiveDialogueQueue(new DialogueNode[] {
            petDialogues[Random.Range(0, petDialogues.Length)],
            dogoPetDialogues[Random.Range(0, petDialogues.Length)]
        }, delegate { EndCombat(true); });
    }

    public void Wait() {
        HideCombatInterface();
        InteractiveDialogueQueue(new DialogueNode[] {
            waitDialogues[Random.Range(0, waitDialogues.Length)],
            dogoWaitDialogues[Random.Range(0, petDialogues.Length)]
        }, delegate { ShowCombatInterface(); });
    }

    public void Surrend() {
        HideCombatInterface();
        InteractiveDialogueQueue(new DialogueNode[] {
            surrendDialogues[Random.Range(0, surrendDialogues.Length)],
            dogoSurrendDialogues[Random.Range(0, petDialogues.Length)]
        }, delegate { EndCombat(false); });
    }

    public void Victory() {
        InteractiveDialogue(victoryDialogue, delegate { EndInteractiveRoutine(victoryNodeId); });
    }

    public void Defeat() {
        InteractiveDialogue(defeatDialogue, delegate { EndInteractiveRoutine(defeatNodeId); });
    }

    private void ShowCombatInterface() {
        combatUI.Show();
    }

    private void HideCombatInterface() {
        combatUI.Hide();
    }

    public long victoryNodeId = -1;
    public long defeatNodeId = -1;

    public CombatUI combatUI;
    public DialogueNode startDialogue;
    public DialogueNode[] petDialogues;
    public DialogueNode[] dogoPetDialogues;
    public DialogueNode[] waitDialogues;
    public DialogueNode[] dogoWaitDialogues;
    public DialogueNode[] surrendDialogues;
    public DialogueNode[] dogoSurrendDialogues;
    public DialogueNode victoryDialogue;
    public DialogueNode defeatDialogue;
}
