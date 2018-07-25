using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasePieceIDs", menuName = "Database/BasePieceIDs", order = 1)]
public class BasePieceIDHolder : ScriptableObject {

    public int max_id {
        get { return _max_id; }
    }
    public int entrance_id {
        get { return _entrance_id;  }
    }
    public int exit_id {
        get { return _exit_id; }
    }

    [SerializeField] List<BasePiece> pieces;
    [SerializeField] int _max_id, _entrance_id, _exit_id;

    public BasePiece GetBasePieceFromID(int id) {
        foreach (BasePiece piece in pieces) {
            if (piece.id == id) {
                return piece;
            }
        }
        return null;
    }
    
}
