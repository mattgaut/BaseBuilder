using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour, IItem {

    [SerializeField] Vector2Int _size;
    [SerializeField] Vector2Int _anchor;

    [SerializeField] int _id;
    [SerializeField] string _group_name;

    [SerializeField] int _price;
    [SerializeField] int _gold_value;

    [SerializeField] List<EnemySpawner> spawners;

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

    public int price {
        get { return _price; }
    }

    public int enemy_count {
        get { return spawners.Count; }
    }

    public List<Enemy> SpawnEnemies() {
        List<Enemy> enemies = new List<Enemy>();
        foreach (EnemySpawner spawner in spawners) {
            enemies.Add(spawner.Spawn());
        }
        return enemies;
    }

    public int GetExpValue() {
        int total = 0;
        foreach (EnemySpawner es in spawners) {
            total += es.enemy.exp_value;
        }
        return total;
    }
    public int GetGoldValue() {
        int total = 0;
        foreach (EnemySpawner es in spawners) {
            total += es.enemy.gold_value;
        }
        return total;
    }
}
