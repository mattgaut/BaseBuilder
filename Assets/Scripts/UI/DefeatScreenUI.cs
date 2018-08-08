using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatScreenUI : MonoBehaviour {

    [SerializeField] GameObject holder;

    [SerializeField] Text level_text, exp_text;
    [SerializeField] Slider level_slider;

    [SerializeField] Button return_to_hub_button;

    public void DisplayScreen(int exp_earned, Account account) {
        holder.SetActive(true);

        level_text.text = account.level + "";
        level_slider.value = ((float)account.experience / account.experience_to_next_level);

        exp_text.text = exp_earned + "";
    }

    private void Awake() {
        return_to_hub_button.onClick.AddListener(() => SceneManager.LoadScene("HUBMenu"));
    }
}
