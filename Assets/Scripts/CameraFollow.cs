using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] Transform follow_object;
    [SerializeField] Vector3 offset;
    [SerializeField] bool set_offset_on_awake;

    private void Awake() {
        if (set_offset_on_awake) offset = transform.position - follow_object.transform.position;
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.position = follow_object.transform.position + offset;
	}
}
