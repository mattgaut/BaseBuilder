using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySkillTree : MonoBehaviour {

    [SerializeField] Text available_points;
    [SerializeField] Slider slider;

    [SerializeField] List<DisplaySkill> display_skills;

    SkillTree skill_tree;

    public void Display(SkillTree skill_tree) {
        this.skill_tree = skill_tree;

        slider.value = skill_tree.spent_skill_points;

        available_points.text = "Available Skill Points: " + skill_tree.available_skill_points;
    }

    private void Awake() {
        foreach (DisplaySkill display_skill in display_skills) {
            display_skill.button.onClick.AddListener((() => LevelSkill(display_skill)));
        }

        if (AccountHolder.has_valid_account) {
            Display(AccountHolder.account.skill_tree);
        }
    }

    void LevelSkill(DisplaySkill display_skill) {
        if (skill_tree != null) {
            skill_tree.TryLevelSkill(display_skill.skill);
            display_skill.Display(display_skill.skill);
            Display(skill_tree);
        }
    }
}
