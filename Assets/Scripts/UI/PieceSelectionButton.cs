using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelectionButton : MonoBehaviour {

    public Button button {
        get { return _button; }
    }
    public Sprite sprite {
        get { return image.sprite; }
        set { image.sprite = value; }
    } 
    public string title {
        get { return _title.text; }
        set { _title.text = value; }
    }
    public int count_max {
        get {
            return _count_max;
        }
        set {
            _count_max = value;
            _count_text.text = _count + " / " + _count_max;
        }
    }
    public int count {
        get {
            return _count;
        }
        set {
            _count = value;
            _count_text.text = _count + " / " + _count_max;
        }
    }

    [SerializeField] Button _button;
    [SerializeField] Image image;
    [SerializeField] Text _title, _description, _count_text;
    int _count, _count_max;

    public void ShowCount(bool enabled) {
        _count_text.enabled = enabled;
    }
}
