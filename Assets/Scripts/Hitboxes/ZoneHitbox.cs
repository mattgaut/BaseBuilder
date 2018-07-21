using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneHitbox : Hitbox {

    public static IEnumerator Timer(float wait) {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(wait);
        yield return new WaitForFixedUpdate();
    }

    public delegate void OnZoneEnter(IDamageable hit);
    public delegate void OnZoneLeave(IDamageable hit);

    float tick_rate;

    bool track_stay;

    List<GameObject> inside;

    OnZoneEnter on_enter;
    OnZoneLeave on_leave;

    public void Init(OnZoneEnter enter, OnZoneLeave leave) {
        on_enter = enter;
        on_leave = leave;
        track_stay = false;
    }

    public void Init(OnHit hit, OnZoneEnter enter, OnZoneLeave leave) {
        Init(hit);
        on_enter = enter;
        on_leave = leave;
        track_stay = false;
    }
    public void Init(OnHit hit, OnZoneEnter enter, OnZoneLeave leave, float tick_rate) {
        Init(hit);
        on_enter = enter;
        on_leave = leave;
        this.tick_rate = tick_rate;
        track_stay = true;
    }

    public void Init(OnHit hit, float tick_rate) {
        Init(hit);
        this.tick_rate = tick_rate;
        track_stay = true;
    }

    protected override void Awake() {
        base.Awake();
        inside = new List<GameObject>();
    }

    protected override void ClearLogs() {
        base.ClearLogs();
        if (on_leave != null) {
            foreach (GameObject go in inside) {
                on_leave(go.GetComponent<IDamageable>());
            }
        }
        inside.Clear();
    }

    protected override void LogHit(GameObject hit) {
        base.LogHit(hit);
        inside.Add(hit);
    }

    protected override void OnTriggerEnter(Collider other) {
        if (hitmask.Contains(other.gameObject.layer)) {
            if (on_enter != null) on_enter(other.gameObject.GetComponent<IDamageable>());
            if (CheckHit(other.attachedRigidbody.gameObject)) {
                Hit(other.attachedRigidbody.GetComponent<IDamageable>());
            } else if (Time.time - tracked_hits[other.attachedRigidbody.gameObject] > tick_rate) {
                Hit(other.attachedRigidbody.GetComponent<IDamageable>());
                tracked_hits[other.attachedRigidbody.gameObject] = Time.time;
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other) {
        if (track_stay && hitmask.Contains(other.gameObject.layer)) {
            if (Time.time - tracked_hits[other.attachedRigidbody.gameObject] > tick_rate) {
                Hit(other.attachedRigidbody.GetComponent<IDamageable>());
                tracked_hits[other.attachedRigidbody.gameObject] += tick_rate;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (hitmask.Contains(other.gameObject.layer)) {
            ClearHit(other.gameObject);
        }
    }

    protected void ClearHit(GameObject hit) {
        inside.Remove(hit);
        if (on_leave != null) on_leave(hit.GetComponent<IDamageable>());
    }
}
