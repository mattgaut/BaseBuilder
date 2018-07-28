using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUBMenuUI : MonoBehaviour {

    [SerializeField] Text account_name_text, current_base_text;
    [SerializeField] Button base_editor;

    [SerializeField] RectTransform button_holder;
    [SerializeField] PieceSelectionButton selection_prefab;

    Account account;

    // Use this for initialization
    void Start () {
		if (AccountHolder.has_valid_account) {
            account = AccountHolder.account;
            account_name_text.text = account.account_name;
            current_base_text.text = account.home_base == null ? "No Home Base" : "Current Base: " + account.home_base.name;
            SetInventory();
        } else {
            current_base_text.text = "No Home Base";
        }

        base_editor.onClick.AddListener(() => BaseEditorOnClick());
	}

    void BaseEditorOnClick() {
        SceneManager.LoadScene("BaseBuildScene");
    }

    void SetInventory() {
        foreach (int base_piece_id in account.base_inventory.AllAvailablePieces()) {
            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            BasePiece piece_to_emulate = Database.base_pieces.GetBasePieceFromID(base_piece_id);
            new_button.title = piece_to_emulate.piece_name;
            new_button.total = account.base_inventory.GetBasePieceCount(base_piece_id);
            button_holder.sizeDelta += Vector2.up * new_button.GetComponent<RectTransform>().rect.height;
        }
    }
}
