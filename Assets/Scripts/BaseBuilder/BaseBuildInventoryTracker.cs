using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuildInventoryTracker : MonoBehaviour {

    [SerializeField] BaseInventory tracked_inventory;
    bool tracking;

    int[] placed_pieces;
    int[] placed_groups;

    private void Awake() {
        SetTrackedInventory(tracked_inventory);
    }

    public void SetTrackedInventory(BaseInventory inventory) {
        tracked_inventory = inventory;
        tracking = tracked_inventory != null;

        if (tracking) {
            placed_pieces = new int[inventory.max_base_piece_id + 1];
            placed_groups = new int[inventory.max_enemy_group_id + 1];
        }
    }

    public bool CanPlacePiece(int piece_id) {
        if (!tracking) {
            return true;
        } else {
            return placed_pieces[piece_id] < tracked_inventory.GetBasePieceCount(piece_id);
        }
    }
    public bool CanPlaceGroup(int group_id) {
        if (!tracking) {
            return true;
        } else {
            return placed_groups[group_id] < tracked_inventory.GetEnemyGroupCount(group_id);
        }
    }

    public void NotePiecePlacement(int piece_id) {
        if (!tracking) return;
        placed_pieces[piece_id]++;
    }
    public void NoteGroupPlacement(int group_id) {
        if (!tracking) return;
        placed_groups[group_id]++;
    }
    public void NotePieceRemoval(int piece_id) {
        if (!tracking) return;
        placed_pieces[piece_id]--;
    }
    public void NoteGroupRemoval(int group_id) {
        if (!tracking) return;
        placed_groups[group_id]--;
    }

    public int PiecesPlaced(int piece_id) {
        if (!tracking) return 0;
        return placed_pieces[piece_id];
    }
    public int GroupsPlaced(int group_id) {
        if (!tracking) return 0;
        return placed_groups[group_id];
    }
}
