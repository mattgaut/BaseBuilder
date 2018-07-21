using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreateZoneOnMouse : CooldownAbility {

    [SerializeField] ZoneHitbox zone_prefab;
    [SerializeField] float tick_rate, zone_length;

    protected override void PerformAbility() {
        CreateZone();
    }

    protected void CreateZone() {
        ZoneHitbox new_zone = Instantiate(zone_prefab, PlayerController.GetMousePosition(), Quaternion.identity);

        new_zone.Init(OnHit, OnEnter, OnLeave, tick_rate);

        StartCoroutine(DestroyAfterWait(new_zone, zone_length));
    }

    protected IEnumerator DestroyAfterWait(ZoneHitbox to_destroy, float wait) {
        yield return ZoneHitbox.Timer(wait);
        Destroy(to_destroy.gameObject);
    }

    protected virtual void OnHit(IDamageable hit) { }
    protected virtual void OnEnter(IDamageable hit) { }
    protected virtual void OnLeave(IDamageable hit) { }
}
