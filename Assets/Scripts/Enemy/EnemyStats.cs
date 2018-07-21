using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour , IStats {

    [SerializeField] ResourceStat _health;
    [SerializeField] Stat _speed;

    public ResourceStat health {
        get {
            return _health;
        }
    }
    public Stat speed {
        get {
            return _speed;
        }
    }

    void Awake() {
        _health.Init();
    }
}
