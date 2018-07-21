using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerTrap : ZoneTriggerTrap {

    [SerializeField] float damage_per_tick;

    public override void OnHit(IDamageable hit) {
        hit.TakeDamage(damage_per_tick);
    }

    public override void Trigger() {
        hitbox.gameObject.SetActive(true);
        anim.StartAnimation();
    }

    public override void TriggerOff() {
        hitbox.gameObject.SetActive(false);
        anim.StopAnimation();
    }
}
