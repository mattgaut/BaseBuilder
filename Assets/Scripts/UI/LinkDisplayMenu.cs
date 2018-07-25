using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDisplayMenu : MonoBehaviour {

    [SerializeField] BaseBuildManager build_manager;
    [SerializeField] Transform link_display_holder;
    [SerializeField] LinkDisplay link_display_prefab;

    [SerializeField] Button clear_button;

    Coroutine hover_routine;

    private void Start() {
        build_manager.AddMapChangedListener(() => RefreshUI());
        clear_button.onClick.AddListener(() => build_manager.ClearLinks());
        RefreshUI();
    }

    public void RefreshUI() {
        for (int i = link_display_holder.childCount - 1; i >= 0; i--) {
            Destroy(link_display_holder.GetChild(i).gameObject);
        }

        foreach (KeyValuePair<TriggerableBasePiece, TriggerBasePiece> kvp in build_manager.triggerable_to_trigger) {
            TriggerableBasePiece trap = kvp.Key;
            TriggerBasePiece trigger = kvp.Value;

            LinkDisplay new_link_display = Instantiate(link_display_prefab, link_display_holder);

            new_link_display.trap_title = trap.name;
            new_link_display.trigger_title = trigger.name;

            new_link_display.delete_button.onClick.AddListener(() => build_manager.DeleteLink(trigger, trap));

            new_link_display.SetEnterAction(() => StartHover(trap));
            new_link_display.SetExitAction(() => EndHover());
        }
    }

    void StartHover(TriggerableBasePiece trap) {
        if (hover_routine != null) {
            StopCoroutine(hover_routine);
        }
        hover_routine = StartCoroutine(HoverRoutine(trap));
    }

    IEnumerator HoverRoutine(TriggerableBasePiece trap) {
        while (true) {
            yield return null;
            build_manager.link.SetLinkHighlight(trap);
        }
    }

    void EndHover() {
        StopCoroutine(hover_routine);
    }
}
