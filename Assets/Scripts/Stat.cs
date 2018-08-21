using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField] protected float _base_value;

    [SerializeField] protected float lower_cap, upper_cap;
    [SerializeField] protected bool has_lower_cap, has_upper_cap;

    protected float _multiplier_buff;
    protected float _flat_buff;


    public Stat(float base_value) {
        _base_value = base_value;
        _multiplier_buff = 1f;
        _flat_buff = 0f;
    }

    public Stat(float base_value, float lower_cap, float upper_cap) : this(base_value) {
        this.lower_cap = lower_cap;
        has_lower_cap = true;

        this.upper_cap = upper_cap;
        has_upper_cap = true;
    }

    public Stat(float base_value, float cap, bool is_upper_cap) : this(base_value) {
        if (is_upper_cap) {
            upper_cap = cap;
            has_upper_cap = true;
        } else {
            lower_cap = cap;
            has_lower_cap = true;
        }
    }

    public float base_value {
        get { return _base_value; }
    }

    public float multiplier_buff {
        get { return 1 + _multiplier_buff; }
        set { _multiplier_buff = value - 1; }
    }
    public float flat_buff {
        get { return _flat_buff; }
    }

    public void ApplyFlatBuff(float flat_buff) {
        ApplyBuff(flat_buff, 0);
    }
    public void ApplyMultiplierBuff(float mult_buff) {
        ApplyBuff(0, flat_buff);
    }
    public virtual void ApplyBuff(float flat_buff, float mult_buff) {
        multiplier_buff *=  (1 + mult_buff);
        _flat_buff += flat_buff;
    }

    public void RemoveFlatBuff(float flat_buff) {
        RemoveBuff(flat_buff, 0);
    }
    public void RemoveMultiplierBuff(float mult_buff) {
        RemoveBuff(0, mult_buff);
    }
    public virtual void RemoveBuff(float flat_buff, float mult_buff) {
        multiplier_buff /= (1 + mult_buff);
        _flat_buff -= flat_buff;
    }

    public static implicit operator float(Stat s) {
        return s.value;
    }
}

[System.Serializable]
public class ResourceStat : Stat {

    protected float _resource_value;

    public ResourceStat(float base_value) : base(base_value) {
        resource_value = base_value;
    }

    public void Init() {
        _resource_value = base_value;
    }

    public float AddResource(float value) {
        float old = resource_value;
        resource_value += value;
        return resource_value - old;
    }

    public float SubtractResource(float value) {
        float old = resource_value;
        resource_value -= value;
        return old - resource_value;
    }

    public override void ApplyBuff(float flat_buff, float mult_buff) {
        float old_max = max_value;
        base.ApplyBuff(flat_buff, mult_buff);
        if (max_value < resource_value) {
            resource_value = max_value;
        }
        if (max_value > old_max) {
            resource_value += max_value - old_max;
        }
    }

    public override void RemoveBuff(float flat_buff, float mult_buff) {
        base.RemoveBuff(flat_buff, mult_buff);
    }

    public override float value {
        get {
            return _resource_value;
        }
    }

    public float max_value {
        get {
            { return (base_value + flat_buff) * multiplier_buff; }
        }
    }

    protected float resource_value {
        get { return _resource_value; }
        private set {
            if (value < 0) {
                _resource_value = 0;
            } else if (value > max_value) {
                _resource_value = max_value;
            } else {
                _resource_value = value;
            }
        }
    }
}