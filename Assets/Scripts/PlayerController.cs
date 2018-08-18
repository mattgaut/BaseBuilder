using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour {

    [SerializeField] LayerMask walking_collision_mask;

    Rigidbody body;
    PlayerCharacter character;

    float horizontal, vertical;
    static Vector3 last_mouse_position;

    [SerializeField] float body_width = 0.5f;

    [SerializeField] CameraRig camera_rig;

    Quaternion current_rotation;

    void Awake() {
        body = GetComponent<Rigidbody>();
        character = GetComponent<PlayerCharacter>();

        current_rotation = Quaternion.Euler(0,0,0);
        camera_rig.SetRotation(current_rotation);
    }

    private void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("Ability1")) {
            character.UseAbility1();
        }
        if (Input.GetButtonDown("Ability2")) {
            character.UseAbility2();
        }
        if (Input.GetButtonDown("Ability3")) {
            character.UseAbility3();
        }
        if (Input.GetButtonDown("Ability4")) {
            character.UseAbility4();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            camera_rig.RotateRight();
            current_rotation *= Quaternion.Euler(0, 90, 0);
            body.transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            camera_rig.RotateLeft();
            current_rotation *= Quaternion.Euler(0, -90, 0);
            body.transform.rotation *= Quaternion.Euler(0, -90, 0);
        }
    }

    void FixedUpdate () {
        Turn();
        Move();
    }

    void Turn() {
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouse_ray, out hit, 30f, 1 << LayerMask.NameToLayer("MousePlane"))) {
            last_mouse_position = hit.point;

            Vector3 relative_position = hit.point - body.position;
            relative_position.y = 0;
            body.MoveRotation(Quaternion.LookRotation(relative_position));
        }
    }

    void Move() {
        body.velocity = Vector3.zero;
        Vector3 movement;
        if (!character.displaced) {
            movement = (new Vector3(horizontal, 0, vertical) * character.player_stats.speed * Time.deltaTime);
            movement = current_rotation * movement;
        } else {
            movement = (character.displacement * Time.deltaTime);
        }
        RaycastHit hit;
        if (Physics.Raycast(body.position, movement, out hit, movement.magnitude + body_width, walking_collision_mask)) {
            movement = hit.point - body.position;
            movement.y = 0;
            movement *= (movement.magnitude - body_width);
        }

        body.MovePosition(body.position + movement);
    }

    public static Vector3 GetMousePosition() {
        return last_mouse_position;
    }
}
