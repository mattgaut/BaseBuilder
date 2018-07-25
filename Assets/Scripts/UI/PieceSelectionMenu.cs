using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelectionMenu : MonoBehaviour {

    [SerializeField] BaseBuildInventoryTracker tracker;
    [SerializeField] BaseInventory inventory;
    [SerializeField] BaseBuildManager build_manager;
    [SerializeField] Transform button_holder;
    [SerializeField] PieceSelectionButton selection_prefab;

    private void Start() {
        build_manager.AddMapChangedListener(()=>RefreshUI());
    }

    public void RefreshUI() {
        for (int i = button_holder.childCount - 1; i >= 0; i--) {
            Destroy(button_holder.GetChild(i).gameObject);
        }

        foreach (int base_piece_id in inventory.AllAvailablePieces()) {
            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            BasePiece piece_to_emulate = Database.base_pieces.GetBasePieceFromID(base_piece_id);
            new_button.title = piece_to_emulate.name;
            new_button.count_max = inventory.GetBasePieceCount(base_piece_id);
            new_button.count = new_button.count_max - tracker.PiecesPlaced(base_piece_id);
            new_button.button.onClick.AddListener(() => build_manager.place_blocks.SetSelectedPiece(piece_to_emulate));
        }
    }
}
