using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour {

    static Database instance;

    [SerializeField] BasePieceIDHolder _base_pieces;
    [SerializeField] EnemyGroupIDHolder _enemy_groups;

    public static BasePieceIDHolder base_pieces {
        get { return instance._base_pieces; }
    }
    public static EnemyGroupIDHolder enemy_groups {
        get { return instance._enemy_groups; }
    }

    public static int GetMaxID(ItemType type) {
        if (type == ItemType.BasePiece) {
            return base_pieces.max_id;
        } else if (type == ItemType.EnemyGroup) {
            return enemy_groups.max_id;
        }
        return 0;
    }

    public static IItem GetItem(ItemType type, int id) {
        if (type == ItemType.BasePiece) {
            return base_pieces.GetBasePieceFromID(id);
        } else if (type == ItemType.EnemyGroup) {
            return enemy_groups.GetEnemyGroupFromID(id);
        }
        return null;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
