using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasePieceIDs", menuName = "Database/BasePieceIDs", order = 1)]
public class BasePieceIDHolder : ScriptableObject {

    [SerializeField] List<BasePiece> pieces;

    public BasePiece GetBasePieceFromID(int id) {
        foreach (BasePiece piece in pieces) {
            if (piece.id == id) {
                return piece;
            }
        }
        return null;
    }
    
}
