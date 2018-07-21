using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBasePiece : BasePiece {

    [SerializeField] ZoneHitbox _trigger_hitbox;

    public override bool is_trigger {
        get {
            return true;
        }
    }

    public ZoneHitbox trigger_hitbox {
        get { return _trigger_hitbox; }
    }
}
