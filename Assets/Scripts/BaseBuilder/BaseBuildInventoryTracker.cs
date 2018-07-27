using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuildInventoryTracker : MonoBehaviour {

    public BaseInventory tracked_inventory {
        get { return _tracked_inventory; }
    }
    public bool tracking {
        get { return _tracked_inventory != null && !use_max_inventory; }
    }

    [SerializeField] BaseInventory _tracked_inventory;
    [SerializeField] bool use_max_inventory;


    int[] placed_pieces;
    int[] placed_groups;

    private void Awake() {
        if (AccountHolder.has_valid_account) {
            SetTrackedInventory(AccountHolder.account.base_inventory);
        }
    }

    public void SetTrackedInventory(BaseInventory inventory) {
        _tracked_inventory = inventory;

        if (tracking) {
            placed_pieces = new int[Database.base_pieces.max_id + 1];
            placed_groups = new int[Database.enemy_groups.max_id + 1];
        }
    }

    public void ResetTracking() {
        placed_pieces = new int[Database.base_pieces.max_id + 1];
        placed_groups = new int[Database.enemy_groups.max_id + 1];
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
