using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour {

    public int skill_points { get; private set; }

    public void GainSkillPoint() {
        GainSkillPoints(1);
    }

    public void GainSkillPoints(int to_gain) {
        if (to_gain < 1) {
            return;
        }
        skill_points += to_gain;
    }

    public SkillsData SaveSkills() {
        return new SkillsData(this);
    }

    public void LoadSkills(SkillsData data) {
        skill_points = data.skill_points;
    }
}

[System.Serializable]
public class SkillsData {
    public int skill_points;

    public SkillsData(SkillTree s) {
        skill_points = s.skill_points;
    }
    public SkillsData() {
        skill_points = 0;
    }
}
