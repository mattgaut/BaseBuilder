using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [SerializeField] PlayerCharacter character;

    [SerializeField] MySlider health_bar, cd_1, cd_2, cd_3, cd_4;

    void Awake() {
        
    }

    void Update() {
        health_bar.SetFill(character.player_stats.health, character.player_stats.health.max_value);
    }
}
