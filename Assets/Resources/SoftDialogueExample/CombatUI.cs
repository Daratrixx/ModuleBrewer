using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftDialogue;

public class CombatUI : MonoBehaviour {

    public Button petButton;
    public Button waitButton;
    public Button surrendButton;

    public GameObject pannel;

    public void PetButtonHandler() {
        combatNode.Pet();
    }
    public void WaitButtonHandler() {
        combatNode.Wait();
    }
    public void SurrendButtonHandler() {
        combatNode.Surrend();
    }

    public void Show() {
        petButton.gameObject.SetActive(true);
        waitButton.gameObject.SetActive(true);
        surrendButton.gameObject.SetActive(true);
        pannel.SetActive(true);
    }
    public void Hide() {
        petButton.gameObject.SetActive(false);
        waitButton.gameObject.SetActive(false);
        surrendButton.gameObject.SetActive(false);
        pannel.SetActive(false);
    }

    public DialogueCombatNode combatNode;
}
