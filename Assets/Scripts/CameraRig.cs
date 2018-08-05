using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

    Quaternion rotation;

    private void Update() {
        transform.rotation = rotation;
    }

    public void RotateLeft() {
        rotation *= Quaternion.Euler(0, -90, 0);
    }
    public void RotateRight() {
        rotation *= Quaternion.Euler(0, 90, 0);
    }
    public void SetPosition(Vector3 position) {
        transform.position = position;
    }
    public void SetRotation(Quaternion rotation) {
        this.rotation = rotation;
    }
}
