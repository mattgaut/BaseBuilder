using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceField : CreateZoneOnMouse {

    protected override void OnHit(IDamageable hit) {
        hit.TakeDamage(10);
    }
}
