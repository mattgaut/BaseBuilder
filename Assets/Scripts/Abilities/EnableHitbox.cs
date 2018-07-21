using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnableHitbox : CooldownAbility {

    [SerializeField] Hitbox hitbox;
    [SerializeField] float length_enabled;

    protected override void PerformAbility() {
        StartCoroutine(EnableAttack());
    }

    protected IEnumerator EnableAttack() {
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(length_enabled);
        hitbox.gameObject.SetActive(false);
    }

    protected abstract void OnHit(IDamageable hit);


    private void Awake() {
        hitbox.Init(OnHit);
    }

}
