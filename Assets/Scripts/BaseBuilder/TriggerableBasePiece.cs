using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableBasePiece : BasePiece {

    public override bool needs_trigger {
        get {
            return true;
        }
    }

    ITriggerable trigger_object;

    protected override void OnAwake() {
        trigger_object = live_object.GetComponentInChildren<ITriggerable>();
    }

    public void SetTriggerHitbox(ZoneHitbox hitbox) {
        trigger_object.SetTriggerHitbox(hitbox);
    }
}
