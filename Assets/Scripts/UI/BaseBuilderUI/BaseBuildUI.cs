using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuildUI : MonoBehaviour {

    [SerializeField] Toggle place_button, link_button, group_button;
    [SerializeField] BaseBuildManager builder;

    [SerializeField] RectTransform right_side_panel;
    [SerializeField] Button right_side_panel_button;

    bool right_side_panel_hidden;

    public void Awake() {
        place_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.PlaceBlocks); });
        link_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.Link); });
        group_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.PlaceEnemies); });

        right_side_panel_button.onClick.AddListener(() => ToggleRightSidePanel());

        right_side_panel_hidden = false;
    }

    void ToggleRightSidePanel() {
        right_side_panel_hidden = !right_side_panel_hidden;

        if (right_side_panel_hidden) {
            right_side_panel.position += new Vector3(right_side_panel.rect.width, 0, 0);
        } else {
            right_side_panel.position -= new Vector3(right_side_panel.rect.width, 0, 0);
        }
    }
}
