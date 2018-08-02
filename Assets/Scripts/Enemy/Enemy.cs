using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour, IDamageable, IDisplaceable {

    [SerializeField] int _exp_value, _gold_value;

    EnemyStats _enemy_stats;

    public OnDieEvent on_die_event {
        get; private set;
    }

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
    public int exp_value {
        get { return _exp_value; }
    }
    public int gold_value {
        get { return _gold_value; }
    }

    public float TakeDamage(float damage) {
        stats.health.SubtractResource(damage);
        if (stats.health <= 0) {
            Die();
        }
        return damage;
    }

    public void Displace(Vector3 displace_direction, float length) {
        displacement = displace_direction / length;
        displacement_length = length;
        displaced = true;
    }

    protected void Die() {
        on_die_event.Invoke(this);
        Destroy(gameObject);
    }

    void Awake() {
        enemy_stats = GetComponent<EnemyStats>();
        on_die_event = new OnDieEvent();
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

    [System.Serializable]
    public class OnDieEvent : UnityEvent<Enemy> {
    }
}
