using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftDialogue;


public class DialogueVisualCombat : DialogueInteractiveNode {

    public DialogueVisualCombat(long nodeId, VisualCombatUI combatUI, DialogueEffect[] effects = null) : base(nodeId, effects) {
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

    public void AttackLeft() {
        HideCombatInterface();
        InteractiveDialogue(leftCharacter.attack.GetSpellEffectDialogue(leftCharacter, rightCharacter)
            , delegate { ShowCombatInterface(); });
    }

    public void Spell1Left() {
        HideCombatInterface();
        InteractiveDialogue(leftCharacter.spell1.GetSpellEffectDialogue(leftCharacter, rightCharacter)
            , delegate { ShowCombatInterface(); });
    }

    public void Spell2Left() {
        HideCombatInterface();
        InteractiveDialogue(leftCharacter.spell2.GetSpellEffectDialogue(leftCharacter, leftCharacter)
            , delegate { ShowCombatInterface(); });
    }

    public void AttackRight() {
        HideCombatInterface();
        InteractiveDialogue(rightCharacter.attack.GetSpellEffectDialogue(rightCharacter, leftCharacter)
            , delegate { ShowCombatInterface(); });
    }

    public void Spell1Right() {
        HideCombatInterface();
        InteractiveDialogue(rightCharacter.spell1.GetSpellEffectDialogue(rightCharacter, leftCharacter)
            , delegate { ShowCombatInterface(); });
    }

    public void Spell2Right() {
        HideCombatInterface();
        InteractiveDialogue(rightCharacter.spell2.GetSpellEffectDialogue(rightCharacter, leftCharacter)
            , delegate { ShowCombatInterface(); });
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

    public VisualCombatUI combatUI;
    public DialogueNode startDialogue;
    public DialogueNode victoryDialogue;
    public DialogueNode defeatDialogue;

    public VisualRPG.Character leftCharacter = VisualRPG.Character.GetAkarys();
    public VisualRPG.Character rightCharacter = VisualRPG.Character.GetKrynn();
}
