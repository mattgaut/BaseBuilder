using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CreateEnemyGroup : MonoBehaviour {

    private void OnGUI() {
        if (GUILayout.Button("Create enemy group with selected enemies." + " (" + Selection.gameObjects.Length + " selected)")) {
            CreateSpawner();
        }
    }

    void CreateSpawner() {
        GameObject[] selected_objects = Selection.gameObjects;

        List<Enemy> enemies = new List<Enemy>();

        foreach (GameObject go in selected_objects) {
            if (go.scene.name == null) {
                Debug.LogError("Can not operate on Prefabs");
                return;
            }
            Enemy e = go.GetComponent<Enemy>();
            if (e == null) {
                Debug.LogError("Must select only objects with enemy component.");
                return;
            } else {
                enemies.Add(e);
            }
        }

        EnemyGroup enemy_group = new GameObject().AddComponent<EnemyGroup>();
        enemy_group.name = "Created Enemy Group";

        foreach (Enemy e in enemies) {
            EnemySpawner spawner = new GameObject().AddComponent<EnemySpawner>();
            spawner.name = "Enemy Spawner";

            spawner.transform.SetParent(enemy_group.transform);
            spawner.transform.localPosition = e.transform.position;

            GameObject enemy_model = Instantiate(e.transform.Find("EnemyBody").gameObject, spawner.transform);
        }

        foreach (Enemy e in enemies) {
            DestroyImmediate(e.gameObject);
        }
    }
}
