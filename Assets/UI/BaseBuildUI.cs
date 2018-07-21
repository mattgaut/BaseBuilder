using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuildUI : MonoBehaviour {

    [SerializeField] Button place_button, link_button, group_button;
    [SerializeField] BaseBuildManager builder;

    public void Awake() {
        place_button.onClick.AddListener(() => builder.SwitchMode(BaseBuildManager.BuildMode.PlaceBlocks));
        link_button.onClick.AddListener(() => builder.SwitchMode(BaseBuildManager.BuildMode.Link));
        group_button.onClick.AddListener(() => builder.SwitchMode(BaseBuildManager.BuildMode.PlaceEnemies));
    }

}
