using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlocks : MonoBehaviour {

    [SerializeField] BasePiece border_piece, entrance_piece, exit_piece, wall_mount, trigger;

    BaseBuildManager manager;
    bool active;

    bool placing_entrance;
    bool placing_exit;

    BasePiece selected_piece;

    Vector2Int mouse_position {
        get { return manager.mouse_position; }
    }
    Facing current_facing {
        get { return manager.current_facing; }
    }

    public void Activate() {
        active = true;
        placing_entrance = placing_exit = false;
    }
    public void Deactivate() {
        active = false;
        SetSelectedPiece(null);
    }

    public void SetManager(BaseBuildManager manager) {
        this.manager = manager;
    }

    public void SetSelectedPiece(BasePiece piece) {
        if (piece != null && piece.id == entrance_piece.id) {
            placing_entrance = true;
            placing_exit = false;
        } else if (piece != null && piece.id == exit_piece.id) {
            placing_exit = true;
            placing_entrance = false;
        } else {
            placing_entrance = false;
            placing_exit = false;
        }

        SelectPiece(piece);
    }

    public void SetSelectedToEntrancePiece() {
        SetSelectedPiece(entrance_piece);
    }
    public void SetSelectedToExitPiece() {
        SetSelectedPiece(exit_piece);
    }

    void Update() {
        if (!active) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetSelectedPiece(null);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            if (selected_piece != null) {
                int new_rotation = (((int)current_facing + 90) % 360);
                manager.SetRotation(new_rotation);
                selected_piece.transform.rotation = Quaternion.Euler(0, (int)current_facing, 0);
            }
        }

        if (selected_piece != null) {
            selected_piece.transform.position = mouse_position.ToVector3Int(Vector3Axis.y) * manager.block_size;
            if (placing_entrance || placing_exit) {
                SetPlacementHighlight(manager.OnBounds(mouse_position));
            } else {
                SetPlacementHighlight(manager.CanBePlaced(selected_piece, mouse_position, current_facing));
            }
        }
        if (!manager.mouse_over_ui) {
            if (placing_entrance) {
                if (Input.GetMouseButtonDown(0)) {
                    if (manager.OnBounds(mouse_position)) {
                        if (manager.entrance_position != Vector2Int.zero) manager.ReplacePiece(manager.entrance_position, border_piece);
                        manager.ReplacePiece(mouse_position, entrance_piece, manager.GetBoundaryFacing(mouse_position));
                        SetSelectedPiece(null);
                        placing_entrance = false;
                        manager.ValidateMap();
                    }
                }
            } else if (placing_exit) {
                if (Input.GetMouseButtonDown(0)) {
                    if (manager.OnBounds(mouse_position)) {
                        if (manager.exit_position != Vector2Int.zero) manager.ReplacePiece(manager.exit_position, border_piece);
                        manager.ReplacePiece(mouse_position, exit_piece, manager.GetBoundaryFacing(mouse_position));
                        SetSelectedPiece(null);
                        placing_exit = false;
                        manager.ValidateMap();
                    }
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    if (manager.TryPlacePiece(selected_piece, mouse_position, current_facing)) {
                        manager.ValidateMap();
                        manager.SetRotation(0);
                        selected_piece.transform.rotation = Quaternion.Euler(0, (int)current_facing, 0);
                    }
                }
                if (Input.GetMouseButtonDown(1)) {
                    manager.TryDeletePiece(mouse_position);
                    manager.ValidateMap();
                }
            }
        }

    }

    void SetPlacementHighlight(bool can_place) {
        selected_piece.editor.SetViewMode(can_place ? BasePieceEditor.ViewMode.PlacementClear : BasePieceEditor.ViewMode.PlacementBlocked);
    }

    void SelectPiece(BasePiece piece) {
        if (selected_piece != null) {
            Destroy(selected_piece.gameObject);
        }
        if (piece != null) {
            selected_piece = Instantiate(piece);
            selected_piece.Init(true);
        }
    }
}