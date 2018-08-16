using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour {

    public int total_skill_points { get; private set; }
    public int spent_skill_points { get; private set; }
    public int available_skill_points { get { return total_skill_points - spent_skill_points; } }

    [SerializeField] List<Skill> skills;

    public void GainSkillPoint() {
        GainSkillPoints(1);
    }

    public void GainSkillPoints(int to_gain) {
        if (to_gain < 1) {
            return;
        }
        total_skill_points += to_gain;
    }

    public SkillsData SaveSkills() {
        return new SkillsData(this);
    }

    public void LoadSkills(SkillsData data) {
        total_skill_points = data.skill_points;
        spent_skill_points = 0;
        int i = 0;
        for (; i < skills.Count && i < data.skill_levels.Length; i++) {
            skills[i].LoadLevel(data.skill_levels[i]);
            spent_skill_points += data.skill_levels[i];
        }
        for (; i < skills.Count; i++) {
            skills[i].LoadLevel(0);
        }
    }

    public List<Skill> GetSkillList() {
        return new List<Skill>(skills);
    }
    
    public void InitializeSkills(Account account) {
        foreach (Skill s in skills) {
            if (s.type == Skill.Type.Account) {
                (s as AccountSkill).Initialize(account);
            }
        }
    }

    public void InitializeSkills(PlayerCharacter character) {
        foreach (Skill s in skills) {
            if (s.type == Skill.Type.Account) {
                (s as CharacterSkill).Initialize(character);
            }
        }
    }

    public bool TryLevelSkill(Skill skill) {
        if (available_skill_points <= 0) {
            return false;
        }
        if (skill.TryLevelSkill(spent_skill_points)) {
            spent_skill_points++;
            return true;
        } else {
            return false;
        }
    }

    public void ResetSkills() {
        foreach (Skill skill in skills) {
            skill.Reset();
        }
    }
}

[System.Serializable]
public class SkillsData {
    public int skill_points;
    public int[] skill_levels;

    public SkillsData(SkillTree s) {
        skill_points = s.total_skill_points;

        List<Skill> skills = s.GetSkillList();
        skill_levels = new int[skills.Count]; 
        for (int i = 0; i < skills.Count; i++) {
            skill_levels[i] = skills[i].level;
        }

    }
    public SkillsData() {
        skill_points = 0;
        skill_levels = new int[0];
    }
}
