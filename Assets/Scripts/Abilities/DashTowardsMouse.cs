using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTowardsMouse : CooldownAbility {
    [SerializeField] float dash_time, max_dash_distance;
    protected override void PerformAbility() {
        Vector3 mouse_position = PlayerController.GetMousePosition();
        Vector3 dash = mouse_position - user.transform.position;
        dash.y = 0;

        float projected_dash_time = dash_time;

        dash = dash.normalized * max_dash_distance;

        user.Displace(dash, projected_dash_time);
    }
}
