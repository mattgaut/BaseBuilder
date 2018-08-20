using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBaseSize : AccountSkill {

    [SerializeField] int[] base_size_bonus_at_level;

    protected override void ApplyEffects() {
        int bonus_base_size = 0;
        if (base_size_bonus_at_level.Length < level) {
             bonus_base_size = base_size_bonus_at_level[level - 1];
        }
        applied_to.AddBonusBaseSize(bonus_base_size);
    }

    protected override void RemoveEffects() {
        throw new System.NotImplementedException();
    }
}
