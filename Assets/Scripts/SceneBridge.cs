using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBridge : MonoBehaviour {

    static BaseData base_data_to_load;
    static bool free_build;

    public static void SetBaseData(BaseData bd) {
        base_data_to_load = bd;
    }

    public static BaseData GetBaseData() {
        BaseData to_return = base_data_to_load;
        base_data_to_load = null;
        return to_return;
    }

    public static void SetFreeBuild(bool set) {
        free_build = set;
    }
    public static bool GetFreeBuild() {
        bool to_return = free_build;
        free_build = false;
        return to_return;
    }
}
