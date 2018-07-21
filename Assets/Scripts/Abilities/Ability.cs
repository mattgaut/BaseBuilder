using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    [SerializeField] string _ability_name;

    protected PlayerCharacter user;

    public bool can_be_used {
        get { return CanBeUsed(); }
    }
    public string ability_name {
        get { return _ability_name; }
    }
    
    public bool TryUse() {
        if (CanBeUsed()) {
            Use();
            return true;
        }
        OnFailUse();
        return false;
    }

    protected abstract bool CanBeUsed();

    protected abstract void Use();

    protected virtual void OnFailUse() {
        Debug.Log(ability_name + " cannot be used.");
    }

    protected virtual void OnAwake() {

    }

    private void Awake() {
        user = GetComponentInParent<PlayerCharacter>();
        OnAwake();
    }

}
