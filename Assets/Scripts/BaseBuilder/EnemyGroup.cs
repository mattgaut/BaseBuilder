using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour, IItem {

    [SerializeField] Vector2Int _size;
    [SerializeField] Vector2Int _anchor;

    [SerializeField] int _id;
    [SerializeField] string _group_name;

    List<EnemySpawner> spawners;

    public int id {
        get { return _id; }
    }

    public Vector2Int size {
        get { return _size; }
    }

    public Vector2Int anchor {
        get { return _anchor; }
    }

    public Vector2Int position {
        get; set;
    }

    public Facing facing {
        get; set;
    }

    public string item_name {
        get { return _group_name; }
    }

    private void Awake() {
        spawners = new List<EnemySpawner>(GetComponentsInChildren<EnemySpawner>());
    }

    public List<Enemy> SpawnEnemies() {
        List<Enemy> enemies = new List<Enemy>();
        foreach (EnemySpawner spawner in spawners) {
            enemies.Add(spawner.Spawn());
        }
        return enemies;
    }
}
