using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IStats {
    [SerializeField] ResourceStat _health;
    [SerializeField] Stat _speed, _strength, _intelligence, _dexterity, _vitality, _cooldown_reduction;

    public ResourceStat health {
        get { return _health; }
    }
    public Stat speed { get { return _speed; } }
    public Stat stength { get { return _strength; } }
    public Stat intelligence { get { return _intelligence; } }
    public Stat dexterity { get { return _dexterity; } }
    public Stat vitality { get { return _vitality; } }
    public Stat cooldown_reduction { get { return _cooldown_reduction; } }

    void Awake() {
        _health.Init();
    }
}
