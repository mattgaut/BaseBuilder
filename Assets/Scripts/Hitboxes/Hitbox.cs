using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour {

    public delegate void OnHit(IDamageable hit);

    protected OnHit on_hit;

    protected Dictionary<GameObject, float> tracked_hits;

    [SerializeField] protected LayerMask hitmask;

    public void Init(OnHit on_hit) {
        this.on_hit = on_hit;
    }

    protected virtual void Awake() {
        tracked_hits = new Dictionary<GameObject, float>();
    }

    protected virtual void ClearLogs() {
        tracked_hits.Clear();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (hitmask.Contains(other.gameObject.layer)) {
            if (CheckHit(other.attachedRigidbody.gameObject)) {
                Hit(other.attachedRigidbody.GetComponent<IDamageable>());
            }
        }
    }

    protected void Hit(IDamageable hit) {
        if (on_hit != null) on_hit(hit);
    }

    protected virtual void LogHit(GameObject hit) {
        tracked_hits.Add(hit, Time.time);
    }

    protected bool CheckHit(GameObject hit) {
        if (tracked_hits.ContainsKey(hit)) {
            return false;
        } else {
            LogHit(hit);
            return true;
        }
    }

    private void OnEnable() {
        ClearLogs();
    }
}