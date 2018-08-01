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

    public static int GetMinGoldValue(BaseData base_data) {
        int min = 0;
        foreach (BaseData.BasePieceData piece_data in base_data.base_pieces_by_id) {
            if (piece_data == null) {
                continue;
            }
            BasePiece bp = base_pieces.GetBasePieceFromID(piece_data.id);
            if (bp != null) {
                min += bp.gold_value;
            }
        }
        return min;
    }

    public static void GetGoldValue(BaseData base_data, out int min, out int max) {
        min = max = 0;
        foreach (BaseData.BasePieceData piece_data in base_data.base_pieces_by_id) {
            if (piece_data == null) {
                continue;
            }
            BasePiece bp = base_pieces.GetBasePieceFromID(piece_data.id);
            if (bp != null) {
                min += bp.gold_value;
            }
        }
        foreach (BaseData.EnemyGroupData enemy_group_data in base_data.enemy_group_by_id) {
            if (enemy_group_data == null) {
                continue;
            }
            EnemyGroup eg = enemy_groups.GetEnemyGroupFromID(enemy_group_data.id);
            if (eg != null) {
                max += eg.GetGoldValue();
            }
        }
        max += min;
    }

    public static int GetMinExpValue(BaseData base_data) {
        int min = 0;
        foreach (BaseData.BasePieceData piece_data in base_data.base_pieces_by_id) {
            if (piece_data == null) {
                continue;
            }
            BasePiece bp = base_pieces.GetBasePieceFromID(piece_data.id);
            if (bp != null) {
                min += bp.exp_value;
            }
        }
        return min;
    }

    public static void GetExpValue(BaseData base_data, out int min, out int max) {
        min = max = 0;
        foreach (BaseData.BasePieceData piece_data in base_data.base_pieces_by_id) {
            if (piece_data == null) {
                continue;
            }
            BasePiece bp = base_pieces.GetBasePieceFromID(piece_data.id);
            if (bp != null) {
                min += bp.exp_value;
            }
        }
        foreach (BaseData.EnemyGroupData enemy_group_data in base_data.enemy_group_by_id) {
            if (enemy_group_data == null) {
                continue;
            }
            EnemyGroup eg = enemy_groups.GetEnemyGroupFromID(enemy_group_data.id);
            if (eg != null) {
                max += eg.GetExpValue();
            }
        }
        max += min;
    }

    public static int GetEnemyCount(BaseData base_data) {
        int count = 0;
        foreach (BaseData.EnemyGroupData enemy_group_data in base_data.enemy_group_by_id) {
            if (enemy_group_data == null) {
                continue;
            }
            EnemyGroup eg = enemy_groups.GetEnemyGroupFromID(enemy_group_data.id);
            if (eg != null) {
                count += eg.enemy_count;
            }
        }
        return count;
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
