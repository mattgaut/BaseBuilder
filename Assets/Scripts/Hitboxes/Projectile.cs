using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Hitbox {

    public delegate void OnBreak();

    [SerializeField] LayerMask breakmask;
    [SerializeField] float speed, max_distance;

    float lifetime;

    OnBreak on_break;

    Rigidbody body;

    public void Init(OnHit on_hit, OnBreak on_break, float speed, float max_travel_distance) {
        Init(on_hit);
        this.on_break = on_break;
        this.speed = speed;
        max_distance = max_travel_distance;
    }
    public void Init(OnHit on_hit, OnBreak on_break) {
        Init(on_hit);
        this.on_break = on_break;
    }

    protected override void Awake() {
        base.Awake();
        body = GetComponent<Rigidbody>();
        lifetime = 0;
    }

    private void FixedUpdate() {
        body.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        lifetime += Time.deltaTime;
        if (max_distance < lifetime * speed) {
            Break();
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (breakmask.Contains(other.gameObject.layer)) {
            Break();
        }
    }

    void Break() {
        if (on_break != null) on_break();

        gameObject.SetActive(false);
        Invoke("ClearObject", 1f);
    }

    void ClearObject() {
        Destroy(gameObject);
    }
}
