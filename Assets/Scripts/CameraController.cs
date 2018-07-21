using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] float min_height, max_height, zoom_speed;
    [SerializeField] float speed, shift_speed;

    private void Update() {
        Vector3 movement = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0) {
            movement = new Vector3(horizontal, 0, vertical);
            movement = movement.normalized * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? shift_speed : speed);
        }

        if (Input.GetKey(KeyCode.Q)) {
            movement.y -= zoom_speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E)) {
            movement.y += zoom_speed * Time.deltaTime;
        }

        transform.position += movement;
        if (transform.position.y <= min_height) {
            transform.position = new Vector3(transform.position.x, min_height, transform.position.z);
        }
        if (transform.position.y >= max_height) {
            transform.position = new Vector3(transform.position.x, max_height, transform.position.z);
        }
    }
}
