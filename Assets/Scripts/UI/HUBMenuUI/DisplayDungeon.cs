using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisplayDungeon : MonoBehaviour {

    [SerializeField] Text dungeon_name, gold_value, enemy_count, exp_value;
    [SerializeField] Image preview_image;

    Toggle toggle;

    public bool selected {
        get { return toggle.isOn; }
    }

    public void Display(BaseData dungeon_to_display) {
        dungeon_name.text = dungeon_to_display.name;
        int min, max;
        enemy_count.text = Database.GetEnemyCount(dungeon_to_display) + "";
        Database.GetGoldValue(dungeon_to_display, out min, out max);
        gold_value.text = min + "-" + max;
        Database.GetExpValue(dungeon_to_display, out min, out max);
        exp_value.text = min + "-" + max;

        Texture2D texture2D = new Texture2D(512,512, TextureFormat.RGB24, false);
        texture2D.LoadRawTextureData(dungeon_to_display.preview);
        texture2D.Apply();
        preview_image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.height, texture2D.width), Vector2.one * 0.5f);
    }

    private void Awake() {
        toggle = GetComponent<Toggle>();
    }
}
