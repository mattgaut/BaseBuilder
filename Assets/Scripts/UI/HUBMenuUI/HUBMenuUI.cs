using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUBMenuUI : MonoBehaviour {

    [SerializeField] Text account_name_text, current_base_text;
    [SerializeField] Button base_editor, view_base_inventory;

	// Use this for initialization
	void Start () {
		if (AccountHolder.has_valid_account) {
            Account account = AccountHolder.account;
            account_name_text.text = account.account_name;
            current_base_text.text = account.home_base == null ? "No Home Base" : "Current Base: " + account.home_base.name;
        } else {
            current_base_text.text = "No Home Base";
        }

        base_editor.onClick.AddListener(() => BaseEditorOnClick());
        view_base_inventory.onClick.AddListener(() => DisplayBaseInventory());
	}

    void BaseEditorOnClick() {
        SceneManager.LoadScene("BaseBuildScene");
    }

    void DisplayBaseInventory() {

    }
}
