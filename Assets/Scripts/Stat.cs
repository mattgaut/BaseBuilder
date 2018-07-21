using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField] protected float _base_value;

    public Stat(float base_value) {
        _base_value = base_value;
    }

    public virtual float value {
        get { return _base_value; }
    }

    public float base_value {
        get { return _base_value; }
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