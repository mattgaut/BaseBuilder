using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlaceBlocks), typeof(LinkTraps), typeof(PlaceEnemies))] // Require Build Modes
[RequireComponent(typeof(BaseBuildInventoryTracker))]                            // Require Inventory Tracker
public class BaseBuildManager : MonoBehaviour, IBaseSaveLoad {

    public enum BuildMode { View, PlaceBlocks, Link, PlaceEnemies }

    public bool map_valid { get; private set; }
    public bool free_build { get; private set; }

    public Facing current_facing { get; private set; }

    public Vector2Int entrance_position { get; private set; }
    public Vector2Int exit_position { get; private set; }

    public Vector2Int mouse_position { get; private set; }

    public bool mouse_over_ui {
        get; private set;
    }

    public Dictionary<Vector2Int, TriggerableBasePiece> triggerables { get; private set; }
    public Dictionary<Vector2Int, TriggerBasePiece> triggers { get; private set; }
    public Dictionary<TriggerableBasePiece, TriggerBasePiece> triggerable_to_trigger { get; private set; }
    public Dictionary<TriggerBasePiece, TriggerableBasePiece> trigger_to_triggerable { get; private set; }

    public int block_size { get { return _block_size; } }

    public PlaceBlocks place_blocks {
        get { return _place_blocks;  }
    }
    public LinkTraps link {
        get { return _link; }
    }
    public PlaceEnemies place_enemies {
        get { return _place_enemies; }
    }

    public bool took_input { get; private set; }
    public bool taking_input { get { return place_blocks.selected_piece != null || link.link_waiting || place_enemies.selected_group != null;  } }

    public int max_size { get { return 50; } }
    public int min_size { get { return 10; } }

    [SerializeField] Vector2Int size;
    [SerializeField][Range(1, 10)] int _block_size = 1;
    [SerializeField] int exit_id, entrance_id;

    [SerializeField] BasePiece border_piece;

    [SerializeField] TakeScreenShot screen_shot;

    UnityEvent map_changed_event;

    Dictionary<Vector2Int, BasePiece> point_to_piece;
    Dictionary<BasePiece, Vector2Int> piece_to_anchor;
    Dictionary<Vector2Int, List<Vector2Int>> mount_to_mounted;

    Dictionary<Vector2Int, EnemyGroup> point_to_enemy_group;
    Dictionary<EnemyGroup, Vector2Int> enemy_group_to_anchor;

    EnemyGroup selected_group;

    LayerMask mask;

    BuildMode current_build_mode;

    PlaceBlocks _place_blocks;
    LinkTraps _link;
    PlaceEnemies _place_enemies;

    BaseBuildInventoryTracker inventory_tracker;

    public void SwitchMode(BuildMode mode) {
        if (current_build_mode == BuildMode.PlaceBlocks) {
            _place_blocks.Deactivate();
        } else if (current_build_mode == BuildMode.Link) {
            _link.Deactivate();
        } else if (current_build_mode == BuildMode.PlaceEnemies) {
            _place_enemies.Deactivate();
        }
        current_build_mode = mode;
        if (current_build_mode == BuildMode.PlaceBlocks) {
            _place_blocks.Activate();
        } else if (current_build_mode == BuildMode.Link) {
            _link.Activate();
        } else if (current_build_mode == BuildMode.PlaceEnemies) {
            _place_enemies.Activate();
        }
    }

    public void AddMapChangedListener(UnityAction action) {
        map_changed_event.AddListener(action);
    }

    public BaseData Save() {
        BaseData data = new BaseData(size, piece_to_anchor, enemy_group_to_anchor, entrance_position, exit_position, map_valid);
        data.SetTriggers(triggerable_to_trigger.ToList());
        data.preview = (byte[])screen_shot.GetScreenShot().GetRawTextureData().Clone();

        return data;
    }

    public bool Load(BaseData data) {
        if (data == null) {
            return false;
        }

        ClearMap();

        entrance_position = new Vector2Int(data.entrance_x, data.entrance_y);
        exit_position = new Vector2Int(data.exit_x, data.exit_y);

        size.x = data.width;
        size.y = data.height;
        for (int x = 0; x < data.base_pieces_by_id.GetLength(0); x++) {
            for (int y = 0; y < data.base_pieces_by_id.GetLength(1); y++) {
                BaseData.BasePieceData piece_data = data.base_pieces_by_id[x, y];
                if (piece_data != null && piece_data.id > 0) {
                    PlacePiece(Database.base_pieces.GetBasePieceFromID(piece_data.id), piece_data.position, piece_data.facing);
                }
            }
        }

        for (int i = 0; i < data.triggerables.Length; i++) {
            TriggerBasePiece trigger = triggers[data.triggers[i].position];
            TriggerableBasePiece triggerable = triggerables[data.triggerables[i].position];

            trigger_to_triggerable.Add(trigger, triggerable);
            triggerable_to_trigger.Add(triggerable, trigger);
        }

        foreach (BaseData.EnemyGroupData enemy in data.enemy_group_by_id) {
            if (enemy != null && enemy.id > 0) {
                PlaceEnemy(Database.enemy_groups.GetEnemyGroupFromID(enemy.id), enemy.position, enemy.facing);
            }
        }

        map_valid = ValidateMap();

        return true;
    }

    public void SetRotation(int rotation) {
        if (rotation % 90 == 0) {
            current_facing = (Facing)rotation;
        }
    }

    public bool TryPlacePiece(BasePiece selected_piece, Vector2Int position, Facing facing) {
        if (selected_piece == null || !CanBePlaced(selected_piece, position, facing) || !inventory_tracker.CanPlacePiece(selected_piece.id)) {
            return false;
        }

        PlacePiece(selected_piece, position, facing);
        return true;
    }

    public void ReplacePiece(Vector2Int new_position, BasePiece piece, Facing facing = Facing.up) {
        DeletePiece(new_position);
        PlacePiece(piece, new_position, facing);
    }

    public bool TryDeletePiece(Vector2Int position) {
        if (!InBounds(position)) {
            return false;
        }
        if (!point_to_piece.ContainsKey(position)) {
            return false;
        }

        DeletePiece(position);

        return true;
    }

    public bool TryPlaceEnemy(EnemyGroup selected_group, Vector2Int position, Facing facing) {
        if (selected_group == null || !SpaceAvailable(selected_group, position, facing) || !inventory_tracker.CanPlaceGroup(selected_group.id)) {
            return false;
        }

        PlaceEnemy(selected_group, position, facing);
        return true;
    }

    public bool TryDeleteEnemy(Vector2Int position) {
        if (!InBounds(position)) {
            return false;
        }
        if (!point_to_enemy_group.ContainsKey(position)) {
            return false;
        }

        DeleteEnemy(position);
        return true;
    }

    public void CreateLink(TriggerBasePiece trigger, TriggerableBasePiece triggerable) {
        if (trigger == null || triggerable == null) {
            return;
        }
        if (triggerable_to_trigger.ContainsKey(triggerable) || trigger_to_triggerable.ContainsKey(trigger)) {
            return;
        }
        triggerable_to_trigger.Add(triggerable, trigger);
        trigger_to_triggerable.Add(trigger, triggerable);
        map_changed_event.Invoke();
    }

    public void DeleteLink(TriggerBasePiece trigger, TriggerableBasePiece triggerable) {
        if (triggerable_to_trigger.ContainsKey(triggerable) && triggerable_to_trigger[triggerable] == trigger) {
            triggerable_to_trigger.Remove(triggerable);
        }
        if (trigger_to_triggerable.ContainsKey(trigger) && trigger_to_triggerable[trigger] == triggerable) {
            trigger_to_triggerable.Remove(trigger);
        }
        map_changed_event.Invoke();
    }

    public void ClearLinks() {
        trigger_to_triggerable.Clear();
        triggerable_to_trigger.Clear();
        map_changed_event.Invoke();
    }

    public bool CanBePlaced(BasePiece piece, Vector2Int position, Facing facing) {
        if (piece.must_be_mounted) {
            Vector2Int check = GetAdjacentPosition(position, facing);
            if (!point_to_piece.ContainsKey(check) || !point_to_piece[check].can_be_mounted_on) {
                return false;
            }
        }
        return SpaceAvailable(piece, position, facing) && inventory_tracker.CanPlacePiece(piece.id);
    }

    public bool InBounds(Vector2Int placement) {
        return !(placement.x <= 0 || placement.y <= 0 || placement.x >= size.x + 1 || placement.y >= size.y + 1);
    }
    public bool OnBounds(Vector2Int placement) {
        return (placement.x == 0 || placement.y == 0 || placement.x == size.x + 1 || placement.y == size.y + 1);
    }
    public Facing GetBoundaryFacing(Vector2Int position) {
        if (position.x == 0) {
            return Facing.left;
        }
        if (position.y == 0) {
            return Facing.down;
        }
        if (position.x == size.x + 1) {
            return Facing.right;
        }
        if (position.y == size.y + 1) {
            return Facing.up;
        }
        return Facing.none;
    }

    public bool ValidateMap() {
        map_valid = CheckMapValid();
        return map_valid;
    }

    public bool SetNewSizeAndReload(int x, int y) {
        if (x < min_size || x > max_size) {
            return false;
        }
        if (x < min_size || x > max_size) {
            return false;
        }

        if (!free_build) {
            if (x > AccountHolder.account.total_base_size || y > AccountHolder.account.total_base_size) {
                return false;
            }
        }

        ClearMap();

        size = new Vector2Int(x, y);

        CreateBorderWall();

        return true;
    }

    void Awake() {
        mask = LayerMask.GetMask("MousePlane");
        point_to_piece = new Dictionary<Vector2Int, BasePiece>();
        piece_to_anchor = new Dictionary<BasePiece, Vector2Int>();
        mount_to_mounted = new Dictionary<Vector2Int, List<Vector2Int>>();
        triggerables = new Dictionary<Vector2Int, TriggerableBasePiece>();
        triggers = new Dictionary<Vector2Int, TriggerBasePiece>();
        triggerable_to_trigger = new Dictionary<TriggerableBasePiece, TriggerBasePiece>();
        trigger_to_triggerable = new Dictionary<TriggerBasePiece, TriggerableBasePiece>();
        point_to_enemy_group = new Dictionary<Vector2Int, EnemyGroup>();
        enemy_group_to_anchor = new Dictionary<EnemyGroup, Vector2Int>();

        current_facing = Facing.up;

        _place_blocks = GetComponent<PlaceBlocks>();
        _place_blocks.SetManager(this);
        _place_blocks.Deactivate();

        _link = GetComponent<LinkTraps>();
        _link.SetManager(this);
        _link.Deactivate();

        _place_enemies = GetComponent<PlaceEnemies>();
        _place_enemies.SetManager(this);
        _place_enemies.Deactivate();

        inventory_tracker = GetComponent<BaseBuildInventoryTracker>();

        free_build = SceneBridge.GetFreeBuild();
        if (free_build) inventory_tracker.SetTrackedInventory(null);
        else if (AccountHolder.account) inventory_tracker.SetTrackedInventory(AccountHolder.account.base_inventory);

        map_changed_event = new UnityEvent();
    }

    void Start() {
        BaseData data;
        if (free_build || !AccountHolder.has_valid_account) {
            data = null;
        } else {
            data = AccountHolder.account.home_base;
        }

        if (!Load(data)) {
            CreateBorderWall();
        }
        SwitchMode(BuildMode.PlaceBlocks);
    }
    void OnGUI() {
        GUI.Label(new Rect(0, 60, 100, 20), map_valid ? "Valid" : "Invalid");
    }

    void Update() {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 100f, mask)) {
            mouse_position = (hit.point / block_size).RoundToVector3Int().ToVector2Int(Vector3Axis.y);
        }

        mouse_over_ui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    void LateUpdate() {
        took_input = taking_input;
    }

    void ClearMap() {
        foreach (BasePiece bp in piece_to_anchor.Keys) {
            Destroy(bp.gameObject);
        }
        foreach (EnemyGroup eg in enemy_group_to_anchor.Keys) {
            Destroy(eg.gameObject);
        }
        piece_to_anchor.Clear();
        point_to_piece.Clear();
        triggers.Clear();
        triggerables.Clear();
        trigger_to_triggerable.Clear();
        triggerable_to_trigger.Clear();

        enemy_group_to_anchor.Clear();
        point_to_enemy_group.Clear();

        entrance_position = Vector2Int.zero;
        exit_position = Vector2Int.zero;

        inventory_tracker.ResetTracking();
    }

    void CreateBorderWall() {
        for (int x = 0; x <= size.x + 1; x++) {
            for (int y = 0; y <= size.y + 1; y++) {
                if (x == 0 || y == 0 || x == size.x + 1 || y == size.y + 1) {
                    PlacePiece(border_piece, new Vector2Int(x, y));
                }
            }
        }
    }

    void SelectGroup(EnemyGroup group) {
        if (selected_group != null) {
            Destroy(selected_group.gameObject);
        }
        if (group != null) {
            selected_group = Instantiate(group);
        }
    }

    void PlacePiece(BasePiece piece, Vector2Int position, Facing facing = Facing.up) {
        BasePiece new_piece = Instantiate(piece);
        new_piece.Init(true);
        new_piece.position = position;
        new_piece.facing = facing;
        new_piece.transform.position = (position.ToVector3Int(Vector3Axis.y) - new Vector3Int(new_piece.anchor.x, 0, new_piece.anchor.y)) * block_size;
        new_piece.transform.rotation = Quaternion.Euler(Vector3.up * (int)facing);

        if (InBounds(position)) {
            inventory_tracker.NotePiecePlacement(piece.id);
        }
        if (piece.must_be_mounted) {
            Vector2Int adjacent_position = GetAdjacentPosition(position, facing);
            if (mount_to_mounted.ContainsKey(adjacent_position)) {
                mount_to_mounted[adjacent_position].Add(position);
            } else {
                mount_to_mounted.Add(adjacent_position, new List<Vector2Int>() { position });
            }
        }
        if (piece.is_trigger) {
            triggers.Add(new_piece.position, new_piece.GetComponent<TriggerBasePiece>());
        }
        if (piece.needs_trigger) {
            triggerables.Add(new_piece.position, new_piece.GetComponent<TriggerableBasePiece>());
        }
        if (new_piece.id == exit_id) {
            exit_position = new_piece.position;
        }
        if (new_piece.id == entrance_id) {
            entrance_position = new_piece.position;
        }

        for (int x = 0; x < new_piece.size.x; x++) {
            for (int y = 0; y < new_piece.size.y; y++) {
                point_to_piece.Add(GetRotatedPosition(piece.anchor, new Vector2Int(x, y), facing) + position, new_piece);
            }
        }
        piece_to_anchor.Add(new_piece, position);

        new_piece.editor.SetViewMode(BasePieceEditor.ViewMode.Normal);
        map_changed_event.Invoke();
    }

    void DeletePiece(Vector2Int position) {
        BasePiece piece = point_to_piece[position];

        if (InBounds(position)) {
            inventory_tracker.NotePieceRemoval(piece.id);
        }
        // If piece is mounted on wall remove it from dicitonary noting wall mounts
        if (piece.must_be_mounted) {
            Vector2Int adjacent_position = GetAdjacentPosition(position, piece.facing);
            mount_to_mounted[adjacent_position].Remove(position);
        }
        // If piece has others mounted on it also remove them
        if (mount_to_mounted.ContainsKey(position)) {
            List<Vector2Int> to_remove = new List<Vector2Int>(mount_to_mounted[position]);
            foreach (Vector2Int mount_position in to_remove) {
                DeletePiece(mount_position);
            }
        }

        if (piece.needs_trigger) {
            if (triggerable_to_trigger.ContainsKey(triggerables[piece.position])) {
                trigger_to_triggerable.Remove(triggerable_to_trigger[triggerables[piece.position]]);
                triggerable_to_trigger.Remove(triggerables[piece.position]);
            }
            triggerables.Remove(piece.position);
        }

        if (piece.is_trigger) {
            if (trigger_to_triggerable.ContainsKey(triggers[piece.position])) {
                triggerable_to_trigger.Remove(trigger_to_triggerable[triggers[piece.position]]);
                trigger_to_triggerable.Remove(triggers[piece.position]);
            }
            triggers.Remove(piece.position);
        }

        for (int x = 0; x < piece.size.x; x++) {
            for (int y = 0; y < piece.size.y; y++) {
                point_to_piece.Remove(position + new Vector2Int(x, y) - piece.anchor);
            }
        }
        piece_to_anchor.Remove(piece);

        Destroy(piece.gameObject);
        map_changed_event.Invoke();
    }

    void PlaceEnemy(EnemyGroup enemy, Vector2Int position, Facing facing) {
        EnemyGroup new_enemy = Instantiate(enemy);
        new_enemy.position = position;
        new_enemy.facing = facing;
        new_enemy.transform.position = (position.ToVector3Int(Vector3Axis.y) - new Vector3Int(new_enemy.anchor.x, 0, new_enemy.anchor.y)) * block_size;
        new_enemy.transform.rotation = Quaternion.Euler(Vector3.up * (int)facing);

        if (InBounds(position)) {
            inventory_tracker.NoteGroupPlacement(enemy.id);
        }

        for (int x = 0; x < new_enemy.size.x; x++) {
            for (int y = 0; y < new_enemy.size.y; y++) {
                point_to_enemy_group.Add(GetRotatedPosition(enemy.anchor, new Vector2Int(x, y), facing) + position, new_enemy);
            }
        }
        enemy_group_to_anchor.Add(new_enemy, position);
        map_changed_event.Invoke();
    }

    void DeleteEnemy(Vector2Int position) {
        EnemyGroup group = point_to_enemy_group[position];
        group.transform.position += Vector3.up;

        if (InBounds(position)) {
            inventory_tracker.NoteGroupRemoval(group.id);
        }

        for (int x = 0; x < group.size.x; x++) {
            for (int y = 0; y < group.size.y; y++) {
                point_to_enemy_group.Remove(position + new Vector2Int(x, y) - group.anchor);
            }
        }
        enemy_group_to_anchor.Remove(group);

        Destroy(group.gameObject);
        map_changed_event.Invoke();
    }

    bool SpaceAvailable(BasePiece piece, Vector2Int placement, Facing facing) {
        if (!InBounds(placement)) {
            return false;
        }
        List<Vector2Int> to_check = new List<Vector2Int>();
        for (int x = 0; x < piece.size.x; x++) {
            for (int y = 0; y < piece.size.y; y++) {
                to_check.Add(GetRotatedPosition(piece.anchor, new Vector2Int(x, y), facing) + placement);
            }
        }
        foreach (Vector2Int check in to_check) {
            if (point_to_piece.ContainsKey(check) || (point_to_enemy_group.ContainsKey(check) && piece.blocks_path)) {
                return false;
            }
        }
        return true;
    }

    bool SpaceAvailable(EnemyGroup group, Vector2Int placement, Facing facing) {
        if (!InBounds(placement)) {
            return false;
        }

        List<Vector2Int> to_check = new List<Vector2Int>();
        for (int x = 0; x < group.size.x; x++) {
            for (int y = 0; y < group.size.y; y++) {
                to_check.Add(GetRotatedPosition(group.anchor, new Vector2Int(x, y), facing) + placement);
            }
        }
        foreach (Vector2Int check in to_check) {
            if (point_to_piece.ContainsKey(check) && point_to_piece[check].blocks_path) {
                return false;
            }
            if (point_to_enemy_group.ContainsKey(check)) {
                return false;
            }
        }
        return true;
    }

    bool CheckMapValid() {
        // Checks that entrance and exit are placed
        if (entrance_position == Vector2Int.zero || exit_position == Vector2Int.zero) {
            return false;
        }

        // Performs a naive search to ensure path between entrance and exit
        HashSet<Vector2Int> checked_positions = new HashSet<Vector2Int>();
        List<Vector2Int> positions_to_check = new List<Vector2Int>() { entrance_position };

        while (positions_to_check.Count > 0) {
            Vector2Int checking = positions_to_check[0];
            positions_to_check.RemoveAt(0);
            if (checked_positions.Contains(checking)) {
                continue;
            }
            if (checking == exit_position) {
                return true;
            }
            checked_positions.Add(checking);

            if ((InBounds(checking) && (!point_to_piece.ContainsKey(checking) || !point_to_piece[checking].blocks_path)) || checking == entrance_position) {
                if (!checked_positions.Contains(checking + Vector2Int.right)) {
                    positions_to_check.Add(checking + Vector2Int.right);
                }
                if (!checked_positions.Contains(checking + Vector2Int.left)) {
                    positions_to_check.Add(checking + Vector2Int.left);
                }
                if (!checked_positions.Contains(checking + Vector2Int.up)) {
                    positions_to_check.Add(checking + Vector2Int.up);
                }
                if (!checked_positions.Contains(checking + Vector2Int.down)) {
                    positions_to_check.Add(checking + Vector2Int.down);
                }
            }
        }

        return false;
    }

    Vector2Int GetAdjacentPosition(Vector2Int position, Facing facing) {
        return position + (Quaternion.Euler(0, (int)facing, 0) * Vector3.forward).RoundToVector3Int().ToVector2Int(Vector3Axis.y);
    }

    Vector2Int GetRotatedPosition(Vector2Int anchor, Vector2Int position, Facing facing) {
        return (Quaternion.Euler(0, (int)facing, 0) * (new Vector3(position.x, 0, position.y) - new Vector3(anchor.x, 0, anchor.y))).RoundToVector3Int().ToVector2Int(Vector3Axis.y);
    }
}

[System.Serializable]
public class BaseData {
    public string name;
    public BasePieceData[,] base_pieces_by_id;
    public EnemyGroupData[,] enemy_group_by_id;
    public int height;
    public int width;
    public int entrance_x, entrance_y;
    public int exit_x, exit_y;

    public bool map_valid;

    public Position[] triggers;
    public Position[] triggerables;

    public byte[] preview;

    public BaseData(Vector2Int size, Dictionary<BasePiece, Vector2Int> pieces, Dictionary<EnemyGroup, Vector2Int> enemies, Vector2Int entrance_position, Vector2Int exit_position, bool map_valid) {
        width = size.x;
        height = size.y;

        entrance_x = entrance_position.x;
        entrance_y = entrance_position.y;

        exit_x = exit_position.x;
        exit_y = exit_position.y;

        base_pieces_by_id = new BasePieceData[width + 2, height + 2];

        foreach (KeyValuePair<BasePiece, Vector2Int> kvp in pieces) {
            base_pieces_by_id[kvp.Value.x, kvp.Value.y] = new BasePieceData();
            base_pieces_by_id[kvp.Value.x, kvp.Value.y].id = kvp.Key.id;
            base_pieces_by_id[kvp.Value.x, kvp.Value.y].position = kvp.Value;
            base_pieces_by_id[kvp.Value.x, kvp.Value.y].facing = kvp.Key.facing;
        }

        enemy_group_by_id = new EnemyGroupData[width + 2, height + 2];

        foreach (KeyValuePair<EnemyGroup, Vector2Int> kvp in enemies) {
            enemy_group_by_id[kvp.Value.x, kvp.Value.y] = new EnemyGroupData();
            enemy_group_by_id[kvp.Value.x, kvp.Value.y].id = kvp.Key.id;
            enemy_group_by_id[kvp.Value.x, kvp.Value.y].position = kvp.Value;
            enemy_group_by_id[kvp.Value.x, kvp.Value.y].facing = kvp.Key.facing;
        }

        this.map_valid = map_valid;
    }

    public void SetTriggers(List<KeyValuePair<TriggerableBasePiece, TriggerBasePiece>> pairs) {
        triggers = new Position[pairs.Count];
        triggerables = new Position[pairs.Count];
        for (int i = 0; i < pairs.Count; i++) {
            triggers[i] = new Position(pairs[i].Value.position);
            triggerables[i] = new Position(pairs[i].Key.position);
        }
    }
    public void SetTriggers(List<KeyValuePair<TriggerBasePiece, TriggerableBasePiece>> pairs) {
        triggers = new Position[pairs.Count];
        triggerables = new Position[pairs.Count];
        for (int i = 0; i < pairs.Count; i++) {
            triggers[i] = new Position(pairs[i].Key.position);
            triggerables[i] = new Position(pairs[i].Value.position);
        }
    }
    public int GetPieceCount(int id, bool in_bounds = true) {
        int count = 0;
        foreach (BasePieceData piece in base_pieces_by_id) {
            if (piece != null && piece.id == id && (!in_bounds || InBounds(piece.position))) {
                count++;
            }
        }
        return count;
    }
    public int GetGroupCount() {
        int count = 0;
        foreach (EnemyGroupData group in enemy_group_by_id) {
            if (group != null) {
                count++;
            }
        }
        return count;
    }
    public int GetGroupCount(int id) {
        int count = 0;
        foreach (EnemyGroupData group in enemy_group_by_id) {
            if (group != null && group.id == id) {
                count++;
            }
        }
        return count;
    }
    bool InBounds(Vector2Int position) {
        return !(position.x <= 0 || position.x >= width + 1 || position.y <= 0 || position.y >= height + 1);
    }

    [System.Serializable]
    public class BasePieceData {
        public int id;
        public Facing facing;

        [SerializeField] int pos_x, pos_y;
        public Vector2Int position {
            get {
                return new Vector2Int(pos_x, pos_y);
            }
            set {
                pos_x = value.x;
                pos_y = value.y;
            }
        }
    }

    [System.Serializable]
    public class EnemyGroupData {
        public int id;
        public Facing facing;

        [SerializeField] int pos_x, pos_y;
        public Vector2Int position {
            get {
                return new Vector2Int(pos_x, pos_y);
            }
            set {
                pos_x = value.x;
                pos_y = value.y;
            }
        }
    }

    [System.Serializable]
    public class Position {
        public Position(Vector2Int pos) {
            position = pos;
        }

        [SerializeField] int pos_x, pos_y;
        public Vector2Int position {
            get {
                return new Vector2Int(pos_x, pos_y);
            }
            set {
                pos_x = value.x;
                pos_y = value.y;
            }
        }
    }
}
