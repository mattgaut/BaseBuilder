using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory), typeof(SkillTree))]
public class Account : MonoBehaviour {

    public string account_name { get; private set; }

    public Inventory inventory { get; private set; }
    public BaseInventory base_inventory { get { return inventory.base_inventory; } }
    public SkillTree skill_tree { get; private set; }

    public bool account_loaded { get; private set; }
    public BaseData home_base { get; private set; }
    public ShopData shop { get; private set; }

    public int level { get; private set; }
    public int experience { get; private set; }
    public int experience_to_next_level { get { return level * 100; } }

    public int max_base_size { get; private set; }

    public void SetShopData(ShopData data) {
        shop = data;
    }

    public void Load(AccountData data) {
        account_name = data.account_name;
        inventory.LoadInventory(data.inventory_data);
        home_base = data.home_base;
        account_loaded = true;
        shop = data.shop;

        experience = data.experience;
        level = data.level;

        max_base_size = data.max_base_size;

        skill_tree.ResetSkills();
        skill_tree.LoadSkills(data.skills);
    }

    public AccountData Save() {
        return new AccountData(this);
    }

    public bool SetHomeBase(BaseData data) {
        if (base_inventory.CanBuild(data)) {
            home_base = data;
            return true;
        }
        return false;
    }

    public void GainExperience(int amount) {
        if (amount > 0) {
            experience += amount;
            while (experience > experience_to_next_level) {
                LevelUp();
            }
        }
    }

    private void Awake() {
        inventory = GetComponent<Inventory>();
        skill_tree = GetComponent<SkillTree>();
        account_loaded = false;
    }

    void LevelUp() {
        experience -= experience_to_next_level;
        level += 1;

        skill_tree.GainSkillPoint();
    }
}

[System.Serializable]
public class AccountData {
    public string account_name;
    public InventoryData inventory_data;
    public BaseData home_base;
    public ShopData shop;

    public SkillsData skills;

    public int experience;
    public int level;

    public int max_base_size;

    public AccountData(string account_name) {
        this.account_name = account_name;
        level = 1;
    }

    public AccountData(Account account) {
        account_name = account.account_name;
        inventory_data = account.inventory.SaveInventory();
        home_base = account.home_base;
        shop = account.shop;

        level = account.level;
        experience = account.experience;

        max_base_size = account.max_base_size;

        skills = account.skill_tree.SaveSkills();
    }
}