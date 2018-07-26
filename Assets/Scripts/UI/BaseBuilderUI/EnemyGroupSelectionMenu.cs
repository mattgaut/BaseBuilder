using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupSelectionMenu : MonoBehaviour {

    [SerializeField] BaseBuildInventoryTracker tracker;
    [SerializeField] BaseBuildManager build_manager;
    [SerializeField] Transform button_holder;
    [SerializeField] PieceSelectionButton selection_prefab;

    private void Start() {
        build_manager.AddMapChangedListener(() => RefreshUI());
        RefreshUI();
    }

    public void RefreshUI() {
        for (int i = button_holder.childCount - 1; i >= 0; i--) {
            Destroy(button_holder.GetChild(i).gameObject);
        }

        if (tracker.tracking) {
            InventoryRefresh();
        } else {
            FreeRefresh();
        }
    }

    void InventoryRefresh() {
        foreach (int group_id in tracker.tracked_inventory.AllAvailableEnemyGroups()) {
            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            EnemyGroup enemy_group_to_emulate = Database.enemy_groups.GetEnemyGroupFromID(group_id);
            new_button.title = enemy_group_to_emulate.name;
            new_button.count_max = tracker.tracked_inventory.GetEnemyGroupCount(group_id);
            new_button.count = new_button.count_max - tracker.GroupsPlaced(group_id);
            new_button.button.onClick.AddListener(() => build_manager.place_enemies.SetSelectedGroup(enemy_group_to_emulate));
        }
    }

    void FreeRefresh() {
        for (int group_id = 0; group_id <= Database.enemy_groups.max_id; group_id++) {
            EnemyGroup enemy_group_to_emulate = Database.enemy_groups.GetEnemyGroupFromID(group_id);
            if (enemy_group_to_emulate == null) {
                continue;
            }

            PieceSelectionButton new_button = Instantiate(selection_prefab, button_holder);
            new_button.title = enemy_group_to_emulate.name;
            new_button.ShowCount(false);
            new_button.button.onClick.AddListener(() => build_manager.place_enemies.SetSelectedGroup(enemy_group_to_emulate));
        }
    }
}
