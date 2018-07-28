using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseInventory))]
public class Inventory : MonoBehaviour {

    public BaseInventory base_inventory { get; private set; }
    public int gold { get; private set; }

    private void Awake() {
        base_inventory = GetComponent<BaseInventory>();
    }

    public void LoadInventory(InventoryData data) {
        base_inventory.LoadInventory(data.base_inventory_data);
    }

    public InventoryData SaveInventory() {
        return new InventoryData(this);
    }
}

[System.Serializable]
public class InventoryData {
    public BaseInventoryData base_inventory_data;
    public int gold;

    public InventoryData(Inventory inventory) {
        base_inventory_data = inventory.base_inventory.SaveInventory();
        gold = inventory.gold;
    }

    public InventoryData(int gold) {
        base_inventory_data = BaseInventoryData.empty;
        this.gold = gold;
    }
}
