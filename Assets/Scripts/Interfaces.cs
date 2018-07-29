using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////
// Combat Interfaces
public interface IStats {
    ResourceStat health { get; }
}

public interface IDamageable {
    IStats stats { get; }
    float TakeDamage(float damage);
}

public interface IDisplaceable {
    Vector3 displacement {
        get;
    }
    bool displaced {
        get;
    }
    float displacement_length {
        get;
    }
}

////
// Manager Interfaces
public interface IBaseSave {
    BaseData Save();
}
public interface IBaseLoad {
    bool Load(BaseData data);
}
public interface IBaseSaveLoad : IBaseLoad, IBaseSave {

}
public interface ITriggerable {
    void SetTriggerHitbox(ZoneHitbox hitbox);
}
public interface ITrigger {
    ZoneHitbox trigger_hitbox {
        get;
    }
}

public interface IItem {

    int id { get; }
    string item_name { get; }
    int price { get; }
}