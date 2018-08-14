using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    public string skill_name { get { return _skill_name; } }
    public int level { get; private set; }
    public int max_level { get { return _max_level; } }
    public int required_points { get { return _required_points; } }

    public Skill parent { get; private set; }

    [SerializeField] bool require_parent_max_level;
    [SerializeField] string _skill_name;
    [SerializeField] int _max_level;
    [SerializeField] int _required_points;

    public bool TryLevelSkill(int total_system_points) {
        if (level >= max_level) {
            return false;
        }
        if (parent != null) {
            if (require_parent_max_level) {
                return parent.level == parent.max_level;
            } else {
                return parent.level > 0;
            }
        }
        if (total_system_points >= required_points) {
            level += 1;
            return true;
        }
        return false;
    }

    public void SetParentSkill(Skill parent) {
        this.parent = parent;
    }
}
