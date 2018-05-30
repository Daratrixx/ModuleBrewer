using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyBar : MonoBehaviour {

    public Heck.Character player;
    public Image background;
    public Image lag;
    public Image foreground;
    public Text text;

    private int lastEnergyValue;
    private int currentLagValue;


    // Use this for initialization
    void Start() {
        currentLagValue = lastEnergyValue = player.currentEnergy;
        UpdateEnergyBar();
    }

    // Update is called once per frame
    void Update() {
        UpdateEnergyBar();
    }

    public void UpdateEnergyBar() {
        if (lastEnergyValue > player.currentEnergy) {
            StartCoroutine(DoLag(lastEnergyValue - player.currentEnergy));
        } else if (player.currentEnergy >= currentLagValue) {
            StopAllCoroutines();
            currentLagValue = player.currentEnergy;
        }
        lastEnergyValue = player.currentEnergy;
        background.rectTransform.sizeDelta = new Vector2(player.maxEnergy + 6, background.rectTransform.sizeDelta.y);
        lag.rectTransform.sizeDelta = new Vector2(currentLagValue, lag.rectTransform.sizeDelta.y);
        foreground.rectTransform.sizeDelta = new Vector2(player.currentEnergy, foreground.rectTransform.sizeDelta.y);
        text.text = player.currentEnergy + "/" + player.maxEnergy;
    }

    IEnumerator DoLag(int value) {
        yield return new WaitForSeconds(0.5f);
        while (value > 1) {
            value -= 2;
            currentLagValue -= 2;
            yield return null;
        }
        currentLagValue -= value;
    }
}
