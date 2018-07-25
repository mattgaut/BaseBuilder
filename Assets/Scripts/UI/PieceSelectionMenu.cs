using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelectionMenu : MonoBehaviour {

    [SerializeField] BaseBuildInventoryTracker tracker;
    [SerializeField] BaseBuildManager build_manager;
    [SerializeField] Transform button_holder;
    [SerializeField] PieceSelectionButton selection_prefab;

    [SerializeField] Button entrance_button, exit_button;

    private void Start() {
        build_manager.AddMapChangedListener(()=>RefreshUI());
        RefreshUI();

        entrance_button.onClick.AddListener(()=>build_manager.place_blocks.SetSelectedToEntrancePiece());
        exit_button.onClick.AddListener(()=>build_manager.place_blocks.SetSelectedToExitPiece());
    }

    public void RefreshUI() {
        for (int i = button_holder.childCount - 1; i >= 0; i--) {
            Destroy(button_holder.GetChild(i).gameObject);
        }

        if (tracker.tracking) {
            InventoryRefresh();
        } else {
            FreeRefresh();
        }
    }

    void InventoryRefresh() {
        foreach (int base_piece_id in tracker.tracked_inventory.AllAvailablePieces()) {
            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            BasePiece piece_to_emulate = Database.base_pieces.GetBasePieceFromID(base_piece_id);
            new_button.title = piece_to_emulate.name;
            new_button.count_max = tracker.tracked_inventory.GetBasePieceCount(base_piece_id);
            new_button.count = new_button.count_max - tracker.PiecesPlaced(base_piece_id);
            new_button.button.onClick.AddListener(() => build_manager.place_blocks.SetSelectedPiece(piece_to_emulate));
        }
    }

    void FreeRefresh() {
        for (int base_piece_id = 0; base_piece_id <= Database.base_pieces.max_id; base_piece_id++) {
            if (base_piece_id == Database.base_pieces.entrance_id || base_piece_id == Database.base_pieces.exit_id) {
                continue;
            }

            BasePiece piece_to_emulate = Database.base_pieces.GetBasePieceFromID(base_piece_id);
            if (piece_to_emulate == null) {
                continue;
            }

            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            new_button.title = piece_to_emulate.name;
            new_button.ShowCount(false);
            new_button.button.onClick.AddListener(() => build_manager.place_blocks.SetSelectedPiece(piece_to_emulate));
        }
    }
}
