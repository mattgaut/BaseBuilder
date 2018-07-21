using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePieceAnimator : MonoBehaviour {

    [SerializeField] ParticleSystem particles;

    Coroutine wait_routine;

    public void StartAnimation() {
        particles.Play();
    }

    public void StartAnimation(float time) {
        StartAnimation();
        if (wait_routine != null) {
            StopCoroutine(wait_routine);
        }
        wait_routine = StartCoroutine(EndAfterDuration(time));
    }

    public void StopAnimation() {
        if (wait_routine != null) {
            StopCoroutine(wait_routine);
            wait_routine = null;
        }
        particles.Stop();
    }

    IEnumerator EndAfterDuration(float time) {
        yield return new WaitForSeconds(time);
        StopAnimation();
    }
}
