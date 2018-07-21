using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    public bool player_touching_exit {
        get; private set;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWalkingHitbox")) {
            player_touching_exit = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWalkingHitbox")) {
            player_touching_exit = false;
        }
    }

}
