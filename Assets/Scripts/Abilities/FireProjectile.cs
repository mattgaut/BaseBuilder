using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireProjectile : CooldownAbility {

    [SerializeField] Projectile projectile_prefab;
    [SerializeField] Transform spawn_point;
    [SerializeField] float projectile_speed, max_travel_distance;

    protected override void PerformAbility() {
        LaunchProjectile();
    }

    protected void LaunchProjectile() {
        Projectile new_projectile = Instantiate(projectile_prefab, spawn_point.position, user.transform.rotation);

        new_projectile.Init(OnHit, OnBreak, projectile_speed, max_travel_distance);
    }

    protected abstract void OnHit(IDamageable hit);
    protected abstract void OnBreak();
}
