using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Vector3Axis { x = 0, y = 1, z = 2 }

public static class ExtensionMethods {

    public static bool Contains(this LayerMask mask, int layer_index) {
        return ((1 << layer_index) & mask) != 0;
    }

    public static Vector3Int CastToVector3Int(this Vector3 vector) {
        return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
    }
    public static Vector3Int RoundToVector3Int(this Vector3 vector) {
        return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    public static Vector2Int ToVector2Int(this Vector3Int vector, Vector3Axis to_drop) {
        if (to_drop == Vector3Axis.x) {
            return new Vector2Int(vector.y, vector.z);
        } else if (to_drop == Vector3Axis.y) {
            return new Vector2Int(vector.x, vector.z);
        } else {
            return new Vector2Int(vector.x, vector.y);
        }
    }

    public static Vector3Int ToVector3Int(this Vector2Int vector, Vector3Axis to_add, int fill_with = 0) {
        if (to_add == Vector3Axis.x) {
            return new Vector3Int(fill_with, vector.x, vector.y);
        } else if (to_add == Vector3Axis.y) {
            return new Vector3Int(vector.x, fill_with, vector.y);
        } else {
            return new Vector3Int(vector.x, vector.y, fill_with);
        }
    }
}
