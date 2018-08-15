using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour {

    public enum Type { None, Character, Account }

    public virtual Type type { get { return Type.None; } }

    public string skill_name { get { return _skill_name; } }
    public string description { get { return _description; } }
    public int level { get; private set; }
    public int max_level { get { return _max_level; } }
    public int required_points { get { return _required_points; } }
    public Sprite sprite { get { return _sprite; } }

    public Skill parent { get { return _parent; } }

    [SerializeField] string _skill_name;
    [SerializeField][TextArea] string _description;

    [SerializeField] int _max_level;
    [SerializeField] int _required_points;
    [SerializeField] Sprite _sprite;

    [SerializeField] Skill _parent;

    public bool CanLevel(int total_system_points) {
        if (level >= max_level) {
            return false;
        }
        if (parent != null) {
            return parent.level > 0;
        }
        if (total_system_points >= required_points) {
            return true;
        }
        return false;
    }

    public bool TryLevelSkill(int total_system_points) {
        if (CanLevel(total_system_points)) {
            level += 1;
            return true;
        }
        return false;
    }

    public void SetParentSkill(Skill parent) {
        _parent = parent;
    }
}
