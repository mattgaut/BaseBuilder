using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePiece : MonoBehaviour, IItem {
    [SerializeField] string _piece_name;
    [SerializeField] int _price;
    [SerializeField] int _exp_value;
    [SerializeField] int _gold_value;

    [SerializeField] Vector2Int _size;
    [SerializeField] Vector2Int _anchor;

    [SerializeField] protected GameObject live_object;
    [SerializeField] protected BasePieceEditor editor_object;

    [SerializeField] int _id;

    [SerializeField] bool _blocks_path;

    [SerializeField] bool _can_be_mounted_on;
    [SerializeField] bool _must_be_mounted;

    public string piece_name {
        get { return _piece_name; }
    }
    public string item_name {
        get { return _piece_name; }
    }

    public int id {
        get { return _id; }
    }

    public int price {
        get { return _price; }
    }

    public int exp_value {
        get { return _exp_value; }
    }

    public int gold_value {
        get { return _gold_value; }
    }

    public BasePieceEditor editor {
        get { return editor_object; }
    }

    public Vector2Int size {
        get { return _size; }
    }

    public Vector2Int anchor {
        get { return _anchor; }
    }

    public Vector2Int position {
        get; set;
    }

    public Facing facing {
        get; set;
    }

    public bool blocks_path {
        get { return _blocks_path; }
    }

    public bool can_be_mounted_on {
        get { return _can_be_mounted_on; }
    }
    public bool must_be_mounted {
        get { return _must_be_mounted; }
    }

    public virtual bool needs_trigger {
        get { return false; }
    }
    public virtual bool is_trigger {
        get { return false; }
    }

    void Awake() {
        OnAwake();
    }

    protected virtual void OnAwake() {

    }

    public void Init(bool is_editor) {
        editor_object.gameObject.SetActive(is_editor);
        live_object.SetActive(!is_editor);
    }
}
