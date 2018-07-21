using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRectangle : EnemyProtectHome {

    [SerializeField] ZoneHitbox hitbox;
    [SerializeField] ParticleSystem particles;

    float begin_attack_range = 5f;
    float min_attack_length = 2f, time_between_attacks = 4f;
    float last_attack;

    protected override void OnAwake() {
        base.OnAwake();
        last_attack = Time.time - time_between_attacks;
        hitbox.Init(OnAttackHit, .2f);
        turn_speed = 90f;
    }

    protected override IEnumerator ProtectHome() {
        while (ShouldRetainAggro()) {
            if (Time.time - last_attack > time_between_attacks && Vector3.Distance(target.transform.position, enemy.transform.position) < begin_attack_range) {
                yield return Attack();
            } else {
                input = target.transform.position - enemy.transform.position;
                yield return null;
            }
        }
    }

    protected IEnumerator Attack() {
        last_attack = Time.time;
        face_target = true;
        use_turn_speed = true;
        BeginAttack();
        while (Time.time - last_attack < min_attack_length || Vector3.Distance(target.transform.position, enemy.transform.position) < begin_attack_range) {
            yield return null;
        }
        EndAttack();
        use_turn_speed = false;
        face_target = false;
        last_attack = Time.time;
    }

    protected void BeginAttack() {
        hitbox.gameObject.SetActive(true);
        particles.Play();
    }

    protected void EndAttack() {
        hitbox.gameObject.SetActive(false);
        particles.Stop();
    }

    private void OnAttackHit(IDamageable d) {
        d.TakeDamage(2f);
    }
}
