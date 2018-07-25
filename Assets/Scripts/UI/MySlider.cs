using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour {

    [SerializeField] Image background, foreground;
    [SerializeField] Text overlay_text;

    public void SetFill(float fill) {
        foreground.fillAmount = fill;
        SetText((int)fill + "%");
    }

    public void SetFill(float over, float under) {
        foreground.fillAmount = over / under;
        SetText((int)over + " / " + (int)under);
    }

    public void SetFillCustomText(float fill, string text) {
        foreground.fillAmount = fill;
        SetText(text);
    }

    public void SetText(string text) {
        if (overlay_text) overlay_text.text = text;
    }

    public void SetTextEnabled(bool enabled) {
        if (overlay_text) overlay_text.enabled = enabled;
    }
}
