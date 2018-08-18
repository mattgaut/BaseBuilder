using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownAbility : Ability {

    public float cooldown {
        get { return _cooldown; }
    }
    public float remaining_cooldown {
        get; private set;
    }
    public bool on_cooldown {
        get; private set;
    }

    [SerializeField] float _cooldown;
    [SerializeField] PlayerCharacter controller;

    protected override void OnAwake() {
        base.OnAwake();
        on_cooldown = false;
    }

    private void Update() {
        if (on_cooldown) {
            remaining_cooldown -= Time.deltaTime;
            if (remaining_cooldown < 0) {
                remaining_cooldown = 0;
                on_cooldown = false;
            }
        }
    }

    protected override bool CanBeUsed() {
        return !on_cooldown;
    }
    protected override void Use() {
        StartCooldown();
        PerformAbility();
    }
    protected abstract void PerformAbility();

    protected void StartCooldown() {
        remaining_cooldown = _cooldown * (1 - controller.player_stats.cooldown_reduction);
        on_cooldown = true;
    }
    protected virtual void OnSuccessfulUse() {

    }
    protected override void OnFailUse() {

    }
}
