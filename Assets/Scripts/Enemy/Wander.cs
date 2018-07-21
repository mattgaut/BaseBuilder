using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : EnemyController {
    protected override IEnumerator AIRoutine() {
        while (true) {
            yield return WalkDirectionForLength(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Random.Range(.25f, 4f));
        }
    }

    IEnumerator WalkDirectionForLength(Vector3 direction, float length) {
        while (length > 0) {
            length -= Time.deltaTime;
            input = direction;
            yield return null;
        }
    }
}
