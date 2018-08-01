using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisplayDungeon : MonoBehaviour {

    [SerializeField] Text dungeon_name;

    Toggle toggle;

    public bool selected {
        get { return toggle.isOn; }
    }

    public void Display(BaseData dungeon_to_display) {
        dungeon_name.text = dungeon_to_display.name;
    }

    private void Awake() {
        toggle = GetComponent<Toggle>();
    }
}
