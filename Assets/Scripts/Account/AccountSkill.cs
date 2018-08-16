using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AccountSkill : Skill {

    public override Type type { get { return Type.Account; } }

    protected Account applied_to;

    public void Initialize(Account account) {
        if (applied_to != null) {
            RemoveEffects();
        }
        applied_to = account;
        ApplyEffects();
    }

    protected abstract void ApplyEffects();

    protected abstract void RemoveEffects();

    protected override void BeforeLevelUp() {
        if (applied_to != null) {
            RemoveEffects();
        }
    }

    protected override void AfterLevelUp() {
        if (applied_to != null) {
            ApplyEffects();
        }
    }

}
