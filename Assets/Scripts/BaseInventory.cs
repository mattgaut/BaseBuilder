using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventory : MonoBehaviour {

    public int max_base_piece_id {
        get { return base_pieces != null ? base_pieces.Length - 1: -1; }
    }
    public int max_enemy_group_id {
        get { return enemy_groups != null ? enemy_groups.Length - 1 : -1; }
    }

    [SerializeField] int[] base_pieces;
    [SerializeField] int[] enemy_groups;

    public int GetBasePieceCount(int piece_id) {
        if (piece_id < base_pieces.Length) {
            return base_pieces[piece_id];
        }
        return 0;
    }
    public int GetEnemyGroupCount(int group_id) {
        if (group_id < base_pieces.Length) {
            return enemy_groups[group_id];
        }
        return 0;
    }

    // Returns a list containing the id of every piece the inventory contains one or more of. 
    public List<int> AllAvailablePieces() {
        List<int> piece_ids = new List<int>();
        for (int i = 0; i < base_pieces.Length; i++) {
            if (base_pieces[i] > 0) {
                piece_ids.Add(i);
            }
        }
        return piece_ids;
    }

    // Returns a list containing the id of every group the inventory contains one or more of. 
    public List<int> AllAvailableEnemyGroups() {
        List<int> group_ids = new List<int>();
        for (int i = 0; i < enemy_groups.Length; i++) {
            if (enemy_groups[i] > 0) {
                group_ids.Add(i);
            }
        }
        return group_ids;
    }

    public void AddPiece(int id, int count = 1) {
        if (id < base_pieces.Length && count > 0) {
            base_pieces[id] += count;
        }
    }

    public void AddEnemyGroup(int id, int count = 1) {
        if (id < enemy_groups.Length && count > 0) {
            enemy_groups[id] += count;
        }
    }

    public bool RemovePiece(int id, int count = 1) {
        if (id < base_pieces.Length && count > 0 && count <= base_pieces[id]) {
            base_pieces[id] -= count;
        }
        return false;
    }

    public bool RemoveEnemyGroup(int id, int count = 1) {
        if (id < enemy_groups.Length && count > 0 && count <= enemy_groups[id]) {
            enemy_groups[id] -= count;
        }
        return false;
    }
}
