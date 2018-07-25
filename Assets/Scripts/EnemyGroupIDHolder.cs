using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroupIDs", menuName = "Database/EnemyGroupIDs", order = 2)]
public class EnemyGroupIDHolder : ScriptableObject {

    public int max_id {
        get { return _max_id; }
    }

    [SerializeField] int _max_id;
    [SerializeField] List<EnemyGroup> groups;

    public EnemyGroup GetEnemyGroupFromID(int id) {
        foreach (EnemyGroup group in groups) {
            if (group.id == id) {
                return group;
            }
        }
        return null;
    }
}
