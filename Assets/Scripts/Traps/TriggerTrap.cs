using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerTrap : Trap, ITriggerable {

    [SerializeField] ZoneHitbox trigger;
    [SerializeField] bool while_trigger_down;
    [SerializeField] float duration_enabled_after_trigger;
    [SerializeField] int total_uses = 0;

    int uses;

    Coroutine wait_routine;

    protected bool trigger_down {
        get; private set;
    }

    public void SetTriggerHitbox(ZoneHitbox hitbox) {
        trigger = hitbox;
        if (trigger != null) trigger.Init(OnEnterTriggerZone, OnLeaveTriggerZone);
    }

    public abstract void Trigger();

    public abstract void TriggerOff();

    public abstract void OnHit(IDamageable hit);

    private void OnEnterTriggerZone(IDamageable hit) {
        if (total_uses > 0 && uses >= total_uses) {
            return;
        }
        uses++;
        trigger_down = true;
        Trigger();
        if (!while_trigger_down) {
            if (duration_enabled_after_trigger <= 0) {
                TriggerOff();
            } else {
                StartTimer();
            }
        }
    }
    private void OnLeaveTriggerZone(IDamageable hit) {
        if (!trigger_down) {
            return;
        }
        trigger_down = false;
        if (while_trigger_down) {
            if (duration_enabled_after_trigger <= 0) {
                TriggerOff();
            } else {
                StartTimer();
            }
        }
    }

    private void StartTimer() {
        if (wait_routine != null) {
            StopCoroutine(wait_routine);
        }
        wait_routine = StartCoroutine(WaitForRemainingDuration(duration_enabled_after_trigger));
    }

    IEnumerator WaitForRemainingDuration(float length) {
        yield return new WaitForSeconds(length);
        TriggerOff();
    }


}
