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

    public bool TrySpendGold(int spend) {
        if (spend < 0 || spend > gold) {
            return false;
        }
        gold -= spend;
        return true;
    }

    public void AddGold(int add) {
        gold += add;
    }

    public int GetItemCount(ItemType type, int id) {
        if (type == ItemType.BasePiece) {
            return base_inventory.GetBasePieceCount(id);
        } else if (type == ItemType.EnemyGroup) {
            return base_inventory.GetEnemyGroupCount(id);
        }

        return 0;
    }

    public bool HasItem(ItemType type, int id, int count = 1) {
        if (type == ItemType.BasePiece) {
            return base_inventory.GetBasePieceCount(id) >= count;
        } else if (type == ItemType.EnemyGroup) {
            return base_inventory.GetEnemyGroupCount(id) >= count;
        }

        return false;
    }

    public void AddItem(ItemType type, int id, int count = 1) {
        if (type == ItemType.BasePiece) {
            base_inventory.AddPiece(id, count);
        } else if (type == ItemType.EnemyGroup) {
            base_inventory.AddEnemyGroup(id, count);
        }
    }

    public bool RemoveItem(ItemType type, int id, int count = 1) {
        if (type == ItemType.BasePiece) {
            return base_inventory.RemovePiece(id, count);
        } else if (type == ItemType.EnemyGroup) {
            return base_inventory.RemoveEnemyGroup(id, count);
        }
        return false;
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
