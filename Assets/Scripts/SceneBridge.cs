using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBridge : MonoBehaviour {

    static BaseData base_data_to_load;

    public static void SetBaseData(BaseData bd) {
        base_data_to_load = bd;
    }

    public static BaseData GetBaseData() {
        BaseData to_return = base_data_to_load;
        base_data_to_load = null;
        return to_return;
    }
}
