using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePieceEditor : MonoBehaviour {

    public enum ViewMode { Normal, PlacementBlocked, PlacementClear, Highlight }

    [SerializeField] GameObject model;

    [SerializeField] Material original_material, blocked_material, clear_material, highlight_material;

    List<MeshRenderer> meshes;

    private void Awake() {
        meshes = new List<MeshRenderer>(model.GetComponentsInChildren<MeshRenderer>());
    }

    public void SetViewMode(ViewMode view_mode) {
        if (view_mode == ViewMode.Normal) {
            SetNormal();
        } else if (view_mode == ViewMode.PlacementBlocked) {
            SetPlacementBlocked();
        } else if (view_mode == ViewMode.PlacementClear) {
            SetPlacementClear();
        } else if (view_mode == ViewMode.Highlight) {
            SetHighlight();
        }
    }

    public virtual void SetNormal() {
        foreach (MeshRenderer mesh in meshes) {
            mesh.material = original_material;
        }
    }
    public virtual void SetPlacementBlocked() {
        foreach (MeshRenderer mesh in meshes) {
            mesh.material = blocked_material;
        }
    }
    public virtual void SetPlacementClear() {
        foreach (MeshRenderer mesh in meshes) {
            mesh.material = clear_material;
        }
    }
    public virtual void SetHighlight() {
        foreach (MeshRenderer mesh in meshes) {
            mesh.material = highlight_material;
        }
    }
}
