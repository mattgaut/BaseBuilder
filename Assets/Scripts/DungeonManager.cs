﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour, IBaseLoad {

    bool loaded;

    [SerializeField] PlayerCharacter character;
    [SerializeField] VictoryScreenUI victory_screen;
    [SerializeField] DefeatScreenUI defeat_screen;

    Vector2Int entrance_position, exit_position;
    Exit exit;
    Vector2 size;

    [SerializeField] float scale, block_size;

    bool level_over;

    Dictionary<Vector2Int, BasePiece> pieces;
    List<Enemy> enemies;
    List<Enemy> killed_enemies;

    BaseData loaded_map;

    public bool Load(BaseData data) {
        if (data == null || !data.map_valid) {
            return false;
        }
        loaded_map = data;

        pieces = new Dictionary<Vector2Int, BasePiece>();
        enemies = new List<Enemy>();
        killed_enemies = new List<Enemy>();

        entrance_position = new Vector2Int(data.entrance_x, data.entrance_y);

        exit_position = new Vector2Int(data.exit_x, data.exit_y);

        size.x = data.width;
        size.y = data.height;
        for (int x = 0; x < data.base_pieces_by_id.GetLength(0); x++) {
            for (int y = 0; y < data.base_pieces_by_id.GetLength(1); y++) {
                BaseData.BasePieceData piece_data = data.base_pieces_by_id[x, y];
                if (piece_data != null && piece_data.id > 0) {
                    Vector2Int position = new Vector2Int(x, y);
                    BasePiece piece = SpawnBasePiece(Database.base_pieces.GetBasePieceFromID(piece_data.id), piece_data.position, piece_data.facing);
                    piece.Init(false);
                    pieces.Add(piece_data.position, piece);
                    piece.position = piece_data.position;
                    piece.facing = piece_data.facing;
                    if (position == exit_position) {
                        exit = piece.GetComponentInChildren<Exit>();
                    }
                }
            }
        }


        // Connect Traps to Triggers
        for (int i = 0; i < data.triggers.Length && i < data.triggerables.Length; i++) {
            TriggerableBasePiece triggerable = (TriggerableBasePiece)pieces[data.triggerables[i].position];
            TriggerBasePiece trigger = (TriggerBasePiece)pieces[data.triggers[i].position];
            triggerable.SetTriggerHitbox(trigger.trigger_hitbox);
        }

        foreach (BaseData.EnemyGroupData enemy_data in data.enemy_group_by_id) {
            if (enemy_data != null && enemy_data.id > 0) {
                EnemyGroup group = SpawnEnemyGroup(Database.enemy_groups.GetEnemyGroupFromID(enemy_data.id), enemy_data.position, enemy_data.facing);

                enemies.AddRange(group.SpawnEnemies());
            }
        }

        foreach (Enemy enemy in enemies) {
            enemy.on_die_event.AddListener((Enemy e) => NoteEnemyDeath(e));
        }

        character.transform.position = (Vector3)entrance_position.ToVector3Int(Vector3Axis.y) * scale * block_size;

        return true;
    }

    void Start () {
		if (!Load(SceneBridge.GetBaseData())) {
            SceneManager.LoadScene(0);
            return;
        }
        if (AccountHolder.has_valid_account) {
            AccountHolder.account.skill_tree.InitializeSkills(character);
        }
        character.AddDieEvent(LoseLevel);
    }

    void Update() {
        if (!level_over && exit.player_touching_exit) {
            WinLevel();
        }
    }

    void NoteEnemyDeath(Enemy enemy) {
        killed_enemies.Add(enemy);
    }


    BasePiece SpawnBasePiece(BasePiece bp, Vector2Int position, Facing facing) {
        BasePiece new_piece = Instantiate(bp);
        new_piece.Init(false);
        new_piece.position = position;
        new_piece.facing = facing;
        new_piece.transform.position =  (position.ToVector3Int(Vector3Axis.y) - new Vector3(new_piece.anchor.x * scale, 0, new_piece.anchor.y))* scale * block_size;
        new_piece.transform.localScale = Vector3.one * scale;
        new_piece.transform.rotation = Quaternion.Euler(Vector2.up * (int)facing);

        return new_piece;
    }

    EnemyGroup SpawnEnemyGroup(EnemyGroup enemy, Vector2Int position, Facing facing) {
        EnemyGroup new_enemy_group = Instantiate(enemy);
        new_enemy_group.position = position;
        new_enemy_group.facing = facing;
        new_enemy_group.transform.position = (Vector3)(position.ToVector3Int(Vector3Axis.y) - new_enemy_group.anchor.ToVector3Int(Vector3Axis.y)) * scale * block_size;
        new_enemy_group.transform.rotation = Quaternion.Euler(Vector2.up * (int)facing);

        return new_enemy_group;
    }

    void WinLevel() {
        level_over = true;

        Account account = AccountHolder.account;

        int experience_gained = Database.GetMinExpValue(loaded_map); 
        foreach (Enemy e in killed_enemies) {
            experience_gained += e.exp_value;
        }

        int gold_gained = Database.GetMinGoldValue(loaded_map);
        foreach (Enemy e in killed_enemies) {
            gold_gained += e.gold_value;
        }

        account.inventory.AddGold(gold_gained);
        account.GainExperience(experience_gained);

        victory_screen.DisplayScreen(experience_gained, gold_gained, AccountHolder.account);
    }

    void LoseLevel() {
        level_over = true;

        Account account = AccountHolder.account;

        int experience_gained = 0;
        foreach (Enemy e in killed_enemies) {
            experience_gained += e.exp_value;
        }

        account.GainExperience(experience_gained);

        defeat_screen.DisplayScreen(experience_gained, AccountHolder.account);
    }
}
