using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MySlider))]
public class CooldownAbilityDisplay : MonoBehaviour {

    [SerializeField] CooldownAbility ability;
    MySlider slider;

    private void Awake() {
        slider = GetComponent<MySlider>();
    }

    private void Update() {
        if (!ability.on_cooldown) {
            slider.SetTextEnabled(false);
        } else {
            slider.SetTextEnabled(true);
        }
        slider.SetFillCustomText(ability.remaining_cooldown/(ability.cooldown), ability.remaining_cooldown.ToString("F1") + "s");
    }
}
