using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour, IDamageable, IDisplaceable {

    EnemyStats _enemy_stats;

    public IStats stats {
        get {
            return _enemy_stats;
        }
    }
    public EnemyStats enemy_stats {
        get {
            return _enemy_stats;
        }
        set {
            _enemy_stats = value;
        }
    }

    public Vector3 displacement {
        get; private set;
    }
    public bool displaced {
        get; private set;
    }
    public float displacement_length {
        get; private set;
    }

    public float TakeDamage(float damage) {
        stats.health.SubtractResource(damage);
        if (stats.health <= 0) {
            Die();
        }
        return damage;
    }

    protected void Die() {
        Destroy(gameObject);
    }

    void Awake() {
        enemy_stats = GetComponent<EnemyStats>();
    }

    private void FixedUpdate() {
        if (displaced) {
            displacement_length -= Time.deltaTime;
            if (displacement_length <= 0) {
                displacement_length = 0;
                displaced = false;
            }
        }
    }

    public void Displace(Vector3 displace_direction, float length) {
        displacement = displace_direction / length;
        displacement_length = length;
        displaced = true;
    }
}
