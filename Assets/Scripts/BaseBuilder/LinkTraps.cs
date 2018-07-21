using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTraps : MonoBehaviour {

    BaseBuildManager manager;
    bool active;

    TriggerableBasePiece to_link_triggerable;
    TriggerBasePiece to_link_trigger;

    TriggerableBasePiece triggerable_highlight;
    TriggerBasePiece trigger_highlight;

    Vector2Int mouse_position {
        get { return manager.mouse_position; }
    }

    Dictionary<Vector2Int, TriggerableBasePiece> triggerables { get { return manager.triggerables; } }
    Dictionary<Vector2Int, TriggerBasePiece> triggers { get { return manager.triggers; } }
    Dictionary<TriggerableBasePiece, TriggerBasePiece> triggerable_to_trigger { get { return manager.triggerable_to_trigger; } }
    Dictionary<TriggerBasePiece, TriggerableBasePiece> trigger_to_triggerable { get { return manager.trigger_to_triggerable; } }


    public void Deactivate() {
        active = false;
    }
    public void Activate() {
        active = true;
    }

    public void SetManager(BaseBuildManager manager) {
        this.manager = manager;
    }

    void Update() {
        if (!active) {
            return;
        }

        if (to_link_trigger != null) {
            if (Input.GetMouseButtonDown(0)) {
                if (triggerables.ContainsKey(mouse_position)) {
                    SetToLinkTriggeable(triggerables[mouse_position]);
                    manager.CreateLink(to_link_trigger, to_link_triggerable);
                    ClearToLink();
                }
            } else {
                if (triggerables.ContainsKey(mouse_position)) {
                    SetLinkHighlight(triggerables[mouse_position]);
                } else {
                    ClearLinkHighlight();
                }
            }
        } else if (to_link_triggerable != null) {
            if (Input.GetMouseButtonDown(0)) {
                if (triggers.ContainsKey(mouse_position)) {
                    SetToLinkTrigger(triggers[mouse_position]);
                    manager.CreateLink(to_link_trigger, to_link_triggerable);
                    ClearToLink();
                }
            } else {
                if (triggers.ContainsKey(mouse_position)) {
                    SetLinkHighlight(triggers[mouse_position]);
                } else {
                    ClearLinkHighlight();
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                if (triggers.ContainsKey(mouse_position)) {
                    ClearLinkHighlight();
                    SetToLinkTrigger(triggers[mouse_position]);
                } else if (triggerables.ContainsKey(mouse_position)) {
                    ClearLinkHighlight();
                    SetToLinkTriggeable(triggerables[mouse_position]);
                }
            } else if (Input.GetMouseButtonDown(1)) {
                if (triggers.ContainsKey(mouse_position) && trigger_to_triggerable.ContainsKey(triggers[mouse_position])) {
                    ClearLinkHighlight();
                    manager.DeleteLink(triggers[mouse_position], trigger_to_triggerable[triggers[mouse_position]]);
                } else if (triggerables.ContainsKey(mouse_position) && triggerable_to_trigger.ContainsKey(triggerables[mouse_position])) {
                    ClearLinkHighlight();
                    manager.DeleteLink(triggerable_to_trigger[triggerables[mouse_position]], triggerables[mouse_position]);
                }
            } else {
                if (triggerables.ContainsKey(mouse_position)) {
                    SetLinkHighlight(triggerables[mouse_position]);
                } else if (triggers.ContainsKey(mouse_position)) {
                    SetLinkHighlight(triggers[mouse_position]);
                } else {
                    ClearLinkHighlight();
                }
            }
        }
    }

    void SetToLinkTrigger(TriggerBasePiece trigger) {
        if (trigger_to_triggerable.ContainsKey(trigger)) {
            return;
        }
        to_link_trigger = trigger;
        trigger.editor.SetHighlight();
    }

    void SetToLinkTriggeable(TriggerableBasePiece triggerable) {
        if (triggerable_to_trigger.ContainsKey(triggerable)) {
            return;
        }
        to_link_triggerable = triggerable;
        triggerable.editor.SetHighlight();
    }

    void ClearToLink() {
        if (to_link_trigger != null) to_link_trigger.editor.SetNormal();
        if (to_link_triggerable != null) to_link_triggerable.editor.SetNormal();
        to_link_trigger = null;
        to_link_triggerable = null;
    }

    void SetLinkHighlight(TriggerableBasePiece to_highlight) {
        ClearLinkHighlight();
        triggerable_highlight = to_highlight;
        triggerable_highlight.editor.SetViewMode(BasePieceEditor.ViewMode.Highlight);
        if (triggerable_to_trigger.ContainsKey(to_highlight)) {
            trigger_highlight = triggerable_to_trigger[to_highlight];
            trigger_highlight.editor.SetViewMode(BasePieceEditor.ViewMode.Highlight);
        }
    }
    void SetLinkHighlight(TriggerBasePiece to_highlight) {
        ClearLinkHighlight();
        trigger_highlight = to_highlight;
        trigger_highlight.editor.SetViewMode(BasePieceEditor.ViewMode.Highlight);
        if (trigger_to_triggerable.ContainsKey(to_highlight)) {
            triggerable_highlight = trigger_to_triggerable[to_highlight];
            triggerable_highlight.editor.SetViewMode(BasePieceEditor.ViewMode.Highlight);
        }
    }

    void ClearLinkHighlight() {
        if (triggerable_highlight != null) {
            triggerable_highlight.editor.SetNormal();
        }
        if (trigger_highlight != null) {
            trigger_highlight.editor.SetNormal();
        }
        trigger_highlight = null;
        triggerable_highlight = null;
    }
}
