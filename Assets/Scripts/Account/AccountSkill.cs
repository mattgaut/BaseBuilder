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

    protected override void BeforeChangeLevel() {
        if (applied_to != null) {
            RemoveEffects();
        }
    }

    protected override void AfterChangeLevel() {
        if (applied_to != null) {
            ApplyEffects();
        }
    }

    protected override void OnReset() {
        RemoveEffects();
        applied_to = null;
    }

}
