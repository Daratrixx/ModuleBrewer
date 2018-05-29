using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    public Heck.Character player;
    public Image background;
    public Image lag;
    public Image foreground;

    private int lastHealthValue;
    private int currentLagValue;

    // Use this for initialization
    void Start() {
        currentLagValue = lastHealthValue = player.currentHealth;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update() {
        UpdateHealthBar();
    }

    public void UpdateHealthBar() {
        if (lastHealthValue - player.currentHealth >= 10) {
            StartCoroutine(DoLag(lastHealthValue - player.currentHealth));
        } else if (player.currentHealth < lastHealthValue) {
            currentLagValue -= (lastHealthValue - player.currentHealth);
        } else if (player.currentHealth >= currentLagValue) {
            StopAllCoroutines();
        }
        lastHealthValue = player.currentHealth;
        background.rectTransform.sizeDelta = new Vector2(player.maxHealth / 2 + 6, background.rectTransform.sizeDelta.y);
        lag.rectTransform.sizeDelta = new Vector2(currentLagValue / 2, lag.rectTransform.sizeDelta.y);
        foreground.rectTransform.sizeDelta = new Vector2(player.currentHealth / 2, foreground.rectTransform.sizeDelta.y);
    }

    IEnumerator DoLag(int value) {
        Debug.Log("Start DoLag :" + value);
        yield return new WaitForSeconds(0.5f);
        while(value > 3) {
            value -= 4;
            currentLagValue -= 4;
            yield return null;
        }
        currentLagValue -= value;
        Debug.Log("End DoLag");
    }
}
