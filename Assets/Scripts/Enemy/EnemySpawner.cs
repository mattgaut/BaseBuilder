using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] Enemy to_spawn;

    public Enemy Spawn() {
        Enemy to_return = Instantiate(to_spawn, transform.position, transform.rotation);
        Destroy(gameObject);
        return to_return;
    }

    public void SetToSpawn(Enemy e) {
        to_spawn = e;
    }
}
