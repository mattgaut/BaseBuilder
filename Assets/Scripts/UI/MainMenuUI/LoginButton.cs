using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoginButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Button button {
        get; private set;
    }

    [SerializeField] Text text;

    bool has_account;
    string account_name;

    void Awake() {
        SetNormalText();
        button = GetComponent<Button>();
    }

    public void SetAccount(string account_name) {
        this.account_name = account_name;
        has_account = true;
        SetNormalText();
    }

    public void ClearAccount() {
        has_account = false;
        account_name = "";
        SetNormalText();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        SetHoverText();
    }

    public void OnPointerExit(PointerEventData eventData) {
        SetNormalText();
    }

    void SetHoverText() {
        if (has_account) {
            text.text = "Change Account?";
        } else {
            text.text = "Login";
        }
    }

    void SetNormalText() {
        if (has_account) {
            text.text = "Playing As: " + account_name;
        } else {
            text.text = "Login";
        }
    }
}
