using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHealthBar : MonoBehaviour {

    public Heck.Character character;
    public Image background;
    public Image lag;
    public Image foreground;
    public Text damageDisplayer;

    private int lastHealthValue;
    private int lastDamage;

    private Coroutine resetAlphaCoroutine;
    private bool isHidden = false;

    void Start() {
        Hide();
    }

    void Update() {
        if (CheckHealth()) {
            Show();
            StopAllCoroutines();
            damageDisplayer.text = "" + lastDamage;
            if (resetAlphaCoroutine != null) {
                SetAlpha(1);
                resetAlphaCoroutine = null;
                damageDisplayer.enabled = true;
            }
            StartCoroutine(ResetDamage());
        }
    }

    void LateUpdate() {
        if (!isHidden && character != null)
            ((RectTransform)transform).position = Camera.main.WorldToScreenPoint(character.transform.position + new Vector3(0, 3, 0));
        //transform.LookAt(Camera.main.transform);
    }

    public bool CheckHealth() {
        if (character == null)
            return false;
        if (lastHealthValue > character.currentHealth) {
            lastDamage += lastHealthValue - character.currentHealth;
            lastHealthValue = character.currentHealth;
            foreground.rectTransform.localScale = new Vector3((float)character.currentHealth / (float)character.maxHealth, 1, 1);
            if (!character.isAlive)
                character = null;
            return true;
        }
        lastHealthValue = character.currentHealth;
        return false;
    }

    IEnumerator ResetDamage() {
        Debug.Log("ResetDamage");
        damageDisplayer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        lastDamage = 0;
        damageDisplayer.enabled = false;
        if(character != null)
            lag.rectTransform.localScale = new Vector3((float)character.currentHealth / (float)character.maxHealth, 1, 1);
        else
            lag.rectTransform.localScale = new Vector3(0, 1, 1);
        resetAlphaCoroutine = StartCoroutine(ResetAlpha());
    }

    IEnumerator ResetAlpha() {
        Debug.Log("ResetAlpha");
        yield return new WaitForSeconds(0.5f);
        float alpha = 1;
        while (alpha > 0) {
            alpha -= Time.deltaTime * 2;
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0);
        isHidden = true;
        resetAlphaCoroutine = null;
        Hide();
    }

    private void SetAlpha(float alpha) {
        background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
        lag.color = new Color(lag.color.r, lag.color.g, lag.color.b, alpha);
        foreground.color = new Color(foreground.color.r, foreground.color.g, foreground.color.b, alpha);
    }

    private void Hide() {
        if (!isHidden) {
            isHidden = true;
            background.enabled = false;
            lag.enabled = false;
            foreground.enabled = false;
            damageDisplayer.enabled = false;
        }
    }

    private void Show() {
        if (isHidden) {
            SetAlpha(1);
            isHidden = false;
            background.enabled = true;
            lag.enabled = true;
            foreground.enabled = true;
            damageDisplayer.enabled = true;
        }
    }
}
