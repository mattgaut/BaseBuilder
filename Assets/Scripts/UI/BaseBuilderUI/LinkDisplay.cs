using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LinkDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Button delete_button {
        get { return _delete_button; }
    }
    public Sprite trap_sprite {
        get { return _trap_sprite.sprite; }
        set { _trap_sprite.sprite = value; }
    }
    public Sprite trigger_sprite {
        get { return _trigger_sprite.sprite; }
        set { _trigger_sprite.sprite = value; }
    }
    public string trap_title {
        get { return _trap_title.text; }
        set { _trap_title.text = value; }
    }
    public string trigger_title {
        get { return _trigger_title.text; }
        set { _trigger_title.text = value; }
    }

    Action enter_action, exit_action;

    [SerializeField] Button _delete_button;
    [SerializeField] Image _trap_sprite, _trigger_sprite;
    [SerializeField] Text _trap_title, _trigger_title;

    public void SetEnterAction(Action action) {
        enter_action = action;
    }
    public void SetExitAction(Action action) {
        exit_action = action;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        enter_action();
    }

    public void OnPointerExit(PointerEventData eventData) {
        exit_action();
    }
}
