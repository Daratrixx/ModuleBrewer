using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftDialogue;

public class VisualCombatUI : MonoBehaviour {

    public Button attackLeftButton;
    public Button spell1LeftButton;
    public Button spell2LeftButton;

    public Button attackRightButton;
    public Button spell1RightButton;
    public Button spell2RightButton;

    public GameObject pannel;

    public void AttackLeftButtonHandler() {
        combatNode.AttackLeft();
    }

    public void Spell1LeftButtonHandler() {
        combatNode.Spell1Left();
    }

    public void Spell2LeftButtonHandler() {
        combatNode.Spell2Left();
    }

    public void AttackRightButtonHandler() {
        combatNode.AttackRight();
    }

    public void Spell1RightButtonHandler() {
        combatNode.Spell1Right();
    }

    public void Spell2RightButtonHandler() {
        combatNode.Spell2Right();
    }

    public void Show() {
        pannel.SetActive(true);
    }
    public void Hide() {
        pannel.SetActive(false);
    }

    public DialogueVisualCombat combatNode;
}
