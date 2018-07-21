using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(Rigidbody))]
public abstract class EnemyController : MonoBehaviour {

    [SerializeField] float body_width;
    [SerializeField] LayerMask walking_collision_mask;
    LayerMask line_of_sight_mask;

    [SerializeField] protected PlayerCharacter target;
    [SerializeField] float gain_aggro_range, maintain_aggro_range;
    [SerializeField] bool needs_line_of_sight;

    [SerializeField] Animator animator;

    float line_of_sight_max_distance;

    Coroutine behaviour;

    Rigidbody body;

    protected Enemy enemy { get; private set; }

    protected Vector3 input;
    protected bool face_target;
    protected bool use_turn_speed;
    protected float turn_speed;

    private void Awake() {
        enemy = GetComponent<Enemy>();
        body = GetComponent<Rigidbody>();
        line_of_sight_mask = LayerMask.GetMask("Wall");
        line_of_sight_max_distance = Mathf.Max(gain_aggro_range, maintain_aggro_range);
        if (target == null) {
            target = FindObjectOfType<PlayerCharacter>();
        }
        OnAwake();
    }

    private void FixedUpdate() {
        Vector3 moved_towards = Move();
        if (face_target) {
            Turn(target.transform.position - transform.position);
        } else {
            Turn(moved_towards);
        }
    }

    private void Start () {
        behaviour = StartCoroutine(AIRoutine());
        OnStart();
	}

    private Vector3 Move() {
        body.velocity = Vector3.zero;
        Vector3 movement;
        if (!enemy.displaced) {
            movement = input.normalized * enemy.enemy_stats.speed * Time.deltaTime;
        } else {
            movement = enemy.displacement * Time.deltaTime;
        }

        // Use Raycasting to prevent movement through walls at seams or high speeds
        RaycastHit hit;
        if (Physics.Raycast(body.position, movement, out hit, movement.magnitude + body_width, walking_collision_mask)) {
            movement = hit.point - body.position;
            movement *= (movement.magnitude - body_width);
        }

        body.MovePosition(body.position + movement);
        input = Vector3.zero;
        return movement;
    }

    private void Turn(Vector3 direction) {
        body.angularVelocity = Vector3.zero;
        if (direction != Vector3.zero) {
            if (use_turn_speed) {
                Quaternion rotation = Quaternion.LookRotation(direction);
                body.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, turn_speed * Time.deltaTime));
            } else {
                body.MoveRotation(Quaternion.LookRotation(direction));
            }
        }
    }

    // Use For initialization in base classes
    protected virtual void OnAwake() {

    }
    protected virtual void OnStart() {

    }

    // Overwrite to create AI Behaviour
    protected abstract IEnumerator AIRoutine();

    // Measure distance between target and if necessary get line of sight
    protected virtual bool ShouldGainAggro() {
        if (Vector3.Distance(target.transform.position, enemy.transform.position) <= gain_aggro_range) {
            return needs_line_of_sight ? HasLineOfSight() : true;
        }
        return false;
    }

    // Measure distance between target and if necessary get line of sight
    protected virtual bool ShouldRetainAggro() {
        if (Vector3.Distance(target.transform.position, enemy.transform.position) <= maintain_aggro_range) {
            return needs_line_of_sight ? HasLineOfSight() : true;
        }
        return false;
    }

    protected bool HasLineOfSight() {
        return !Physics.Linecast(enemy.transform.position + Vector3.up, target.transform.position + Vector3.up, line_of_sight_mask);
    }
}
