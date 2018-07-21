using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoneTriggerTrap : TriggerTrap {

    [SerializeField] protected ZoneHitbox hitbox;
    [SerializeField] float tick_rate;

    protected Coroutine timer;

    protected virtual void Awake() {
        hitbox.Init(OnHit, OnZoneEnter, OnZoneExit, tick_rate);
    }

    protected virtual void OnZoneEnter(IDamageable damageable) { }

    protected virtual void OnZoneExit(IDamageable damageable) { }
}
