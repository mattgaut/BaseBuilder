﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerStats))]
public class PlayerCharacter : MonoBehaviour, IDamageable, IDisplaceable {

    [SerializeField] Ability ability_1, ability_2, ability_3, ability_4;
    PlayerStats _player_stats;
    UnityEvent on_die_event;

    public Vector3 displacement {
        get; private set;
    }
    public bool displaced {
        get; private set;
    }
    public float displacement_length {
        get; private set;
    }

    public IStats stats {
        get {
            return _player_stats;
        }
    }
    public PlayerStats player_stats {
        get {
            return _player_stats;
        }
        private set {
            _player_stats = value;
        }
    }

    public bool dead {
        get; private set;
    }

    public void UseAbility1() {
        ability_1.TryUse();
    }

    public void UseAbility2() {
        ability_2.TryUse();
    }

    public void UseAbility3() {
        ability_3.TryUse();
    }

    public void UseAbility4() {
        ability_4.TryUse();
    }

    public void Displace(Vector3 displace_direction, float length) {
        displacement = displace_direction/length;
        displacement_length = length;
        displaced = true;
    }

    private void Awake() {
        player_stats = GetComponent<PlayerStats>();
        on_die_event = new UnityEvent();
    }

    private void FixedUpdate() {
        if (displaced) {
            displacement_length -= Time.deltaTime;
            if (displacement_length <= 0 ) {
                displacement_length = 0;
                displaced = false;
            }
        }
    }

    public float TakeDamage(float damage) {
        float damage_taken = stats.health.SubtractResource(damage);
        if (stats.health.value <= 0) {
            Die();
        }
        return damage;
    }

    public void AddDieEvent(UnityAction action) {
        on_die_event.AddListener(action);
    }

    void Die() {
        if (!dead) {
            dead = true;
            on_die_event.Invoke();
        }
    }
}
