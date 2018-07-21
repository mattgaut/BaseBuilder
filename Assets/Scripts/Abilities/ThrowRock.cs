using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : FireProjectile {

    [SerializeField] int damage;

    protected override void OnBreak() {
        
    }

    protected override void OnHit(IDamageable hit) {
        hit.TakeDamage(damage);
    }

}
