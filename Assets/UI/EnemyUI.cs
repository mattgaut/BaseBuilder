using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour {

    [SerializeField] Canvas canvas;
    [SerializeField] Enemy enemy;
    [SerializeField] MySlider health_bar;

    Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    void Update () {
        Vector3 to_camera = canvas.transform.position - _camera.transform.position;
        to_camera.x = 0;
        canvas.transform.rotation = Quaternion.LookRotation(to_camera);

        health_bar.SetFill(enemy.stats.health, enemy.stats.health.max_value);
	}
}
