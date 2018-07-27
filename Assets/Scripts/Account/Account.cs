using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseInventory))]
public class Account : MonoBehaviour {

    public string account_name { get; private set; }
    public BaseInventory base_inventory { get; private set; }
    public bool account_loaded { get; private set; }

    public void Load(AccountData data) {
        account_name = data.account_name;
        base_inventory.LoadInventory(data.base_inventory_data);
        account_loaded = true;
    }

    public AccountData Save() {
        return new AccountData(this);
    }

    private void Awake() {
        base_inventory = GetComponent<BaseInventory>();
        account_loaded = false;
    }
}

[System.Serializable]
public class AccountData {
    public string account_name;
    public BaseInventoryData base_inventory_data;

    public AccountData(string account_name) {
        this.account_name = account_name;
    }

    public AccountData(Account account) {
        account_name = account.account_name;
        base_inventory_data = account.base_inventory.SaveInventory();
    }
}