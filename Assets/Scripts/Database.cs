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

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

}
