using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProtectHome : EnemyController {

    Vector3 home;

    protected override void OnAwake() {
        base.OnAwake();
        home = transform.position;
    }

    protected override void OnStart() {
        base.OnStart();
    }

    protected override IEnumerator AIRoutine() {
        while (true) {
            yield return WaitForAggro();

            yield return ProtectHome();

            yield return ReturnHome();
        }
    }

    protected virtual IEnumerator WaitForAggro() {
        while (!ShouldGainAggro()) {
            yield return null;
        }
    }

    protected virtual IEnumerator ProtectHome() {
        while (ShouldRetainAggro()) {
            input = target.transform.position - enemy.transform.position;
            yield return null;
        }
    }

    protected virtual IEnumerator ReturnHome() {
        while (Vector3.Distance(enemy.transform.position, home) > 0.05f) {
            input = home - transform.position;
            yield return null;
            if (ShouldGainAggro()) {
                break;
            }
        }
    }
}
