using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEnemies : MonoBehaviour {

    [SerializeField] EnemyGroup one, two, three;

    BaseBuildManager manager;
    bool active;

    EnemyGroup selected_group;

    Vector2Int mouse_position {
        get { return manager.mouse_position; }
    }
    Facing current_facing {
        get { return manager.current_facing; }
    }

    public void Activate() {
        active = true;
    }
    public void Deactivate() {
        active = false;
        SetSelectedGroup(null);
    }
    public void SetManager(BaseBuildManager manager) {
        this.manager = manager;
    }

    void Update() {
        if (!active) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SetSelectedGroup(one);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetSelectedGroup(two);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetSelectedGroup(three);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            if (selected_group != null) {
                int new_rotation = (((int)current_facing + 90) % 360);
                manager.SetRotation(new_rotation);
                selected_group.transform.rotation = Quaternion.Euler(0, (int)current_facing, 0);
            }
        }

        if (selected_group != null) {
            selected_group.transform.position = mouse_position.ToVector3Int(Vector3Axis.y) * manager.block_size;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (manager.TryPlaceEnemy(selected_group, mouse_position, current_facing)) {
                manager.ValidateMap();
                manager.SetRotation(0);
                selected_group.transform.rotation = Quaternion.Euler(0, (int)current_facing, 0);
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            manager.TryDeletePiece(mouse_position);
            manager.ValidateMap();
        }
    }

    void SetSelectedGroup(EnemyGroup group) {
        if (selected_group != null) {
            Destroy(selected_group.gameObject);
        }
        if (group != null) {
            selected_group = Instantiate(group);
        }
    }
}
