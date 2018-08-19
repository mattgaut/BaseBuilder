using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDRSkill", menuName = "Skills/CDRSkill", order = 2)]
public class CharacterCooldownReductionSkill : CharacterSkill {

    [SerializeField] float[] buff_at_level;

    protected override void Initialize() {
        float buff_value = 0;
        if (level <= buff_at_level.Length) {
            buff_value = buff_at_level[level - 1];
        }
        character.player_stats.cooldown_reduction.ApplyFlatBuff(buff_value);
    }
}
