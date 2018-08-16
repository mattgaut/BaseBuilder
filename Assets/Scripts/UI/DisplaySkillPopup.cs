using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplaySkillPopup : MonoBehaviour {

    [SerializeField] Text description, skill_name, levels;

	public void Display(Skill skill) {
        if (skill != null) {
            description.text = skill.description;

            skill_name.text = skill.skill_name;

            if (skill.max_level == 1) {
                levels.text = skill.level == 1 ? "Learned" : "Not Learned";
            } else {
                levels.text = skill.level + "/" + skill.max_level;
            }
        }
    }
}
