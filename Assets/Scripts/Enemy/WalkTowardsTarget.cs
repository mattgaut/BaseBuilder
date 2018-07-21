using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTowardsTarget : EnemyController {
    [SerializeField] Transform target;

    protected override IEnumerator AIRoutine() {
        yield return Walk();
    }

    protected IEnumerator Walk() {
        while (true) {
            Vector3 move_towards = target.position - transform.position;
            move_towards.y = 0;
            input = move_towards;
            yield return null;
        }
    }
}
