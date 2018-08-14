using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    public string skill_name { get { return _skill_name; } }
    public int level { get; private set; }
    public int max_level { get { return _max_level; } }
    public int required_points { get { return _required_points; } }

    [SerializeField] string _skill_name;
    [SerializeField] int _max_level;
    [SerializeField] int _required_points;

    public bool TryLevelSkill() {

    }
}
