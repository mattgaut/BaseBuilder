using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField] protected float _base_value;

    protected float _multiplier_buff = 1;
    protected float _flat_buff = 0;

    public Stat(float base_value) {
        _base_value = base_value;
    }

    public virtual float value {
        get { return _base_value; }
    }

    public float base_value {
        get { return _base_value; }
    }

    public float multiplier_buff {
        get { return _multiplier_buff; }
    }
    public float flat_buff {
        get { return _flat_buff; }
    }

    public void ApplyFlatBuff(float flat_buff) {
        _flat_buff += flat_buff;
    }
    public void ApplyMultiplierBuff(float mult_buff) {
        _multiplier_buff *= mult_buff;
    }
    public void ApplyBuff(float flat_buff, float mult_buff) {
        _multiplier_buff *= mult_buff;
        _flat_buff += flat_buff;
    }

    public void RemoveFlatBuff(float flat_buff) {
        _flat_buff -= flat_buff;
    }
    public void RemoveMultiplierBuff(float mult_buff) {
        _multiplier_buff /= mult_buff;
    }
    public void RemoveBuff(float flat_buff, float mult_buff) {
        _multiplier_buff /= mult_buff;
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

    public override float value {
        get {
            return _resource_value;
        }
    }

    public float max_value {
        get {
            { return base_value; }
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