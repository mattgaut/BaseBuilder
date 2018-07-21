using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : EnableHitbox {
    [SerializeField] int damage;
    protected override void OnHit(IDamageable hit) {
        hit.TakeDamage(damage);
    }
}
