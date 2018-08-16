using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplaySkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Button button { get { return _button; } }

    public Skill skill { get; private set; }

    [SerializeField] Skill to_display;
    [SerializeField] Image sprite_display;
    [SerializeField] DisplaySkillPopup popup_prefab;
    [SerializeField] Button _button;

    DisplaySkillPopup popup;

    public void Display(Skill skill) {
        this.skill = skill;

        sprite_display.sprite = skill.sprite;

        sprite_display.color = skill.level > 0 ? Color.white : Color.gray;

        if (popup != null) {
            popup.Display(skill);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        popup = Instantiate(popup_prefab, transform.parent);
        popup.transform.position = transform.position + new Vector3(sprite_display.rectTransform.rect.width, -sprite_display.rectTransform.rect.height/2, 0);
        popup.Display(skill);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Destroy(popup.gameObject);
    }

    void Awake() {
        if (to_display != null) {
            Display(to_display);
        }
    }
}
