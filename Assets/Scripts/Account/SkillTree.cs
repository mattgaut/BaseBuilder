using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour {

    public int total_skill_points { get; private set; }
    public int spent_skill_points { get; private set; }
    public int available_skill_points { get { return total_skill_points - spent_skill_points; } }

    [SerializeField] List<Skill> skills;
    Dictionary<int, Skill> skills_by_id;

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

        for (int i = 0; i < data.skill_levels.Length; i++) {
            if (skills_by_id.ContainsKey(i)) {
                skills_by_id[i].LoadLevel(data.skill_levels[i]);
                spent_skill_points += data.skill_levels[i];
            }
        }
    }

    public Skill GetSkill(int id) {
        return skills_by_id[id];
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

    private void Awake() {
        skills_by_id = new Dictionary<int, Skill>();
        foreach (Skill skill in skills) {
            Debug.Log(skill.name + " : " + skill.id);
            skills_by_id.Add(skill.id, skill);
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
        int max_id = 0;
        foreach (Skill skill in skills) {
            if (skill.id > max_id) {
                max_id = skill.id;
            }
        }
        skill_levels = new int[max_id + 1]; 
        for (int i = 0; i < skills.Count; i++) {
            skill_levels[skills[i].id] = skills[i].level;
        }
    }
    public SkillsData() {
        skill_points = 0;
        skill_levels = new int[0];
    }
}
