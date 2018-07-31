using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Inventory))]
public class Shop : MonoBehaviour {

    [SerializeField] UnityEvent on_gen_new_shop;

    [SerializeField][Range(0f,1f)] float _buy_rate;
    [SerializeField] int hours_open, minutes_open;
    TimeSpan time_open;

    public DateTime time_created { get; private set; }
    public Inventory inventory { get; private set; }
    public float buy_rate { get { return _buy_rate; } }
    public TimeSpan time_left { get { return time_created.Add(time_open) - DateTime.Now; } }
    

    // Shop sell to Player
    public void SellItem(Inventory selling_to, ItemType type, int item_id, int price, int count = 1) {
        // Cancel if inventory does not have item to sell
        if (!inventory.HasItem(type, item_id, count)) {
            return;
        }
        if (selling_to.TrySpendGold(price * count)) {
            inventory.AddGold(price * count);
            inventory.RemoveItem(type, item_id, count);

            selling_to.AddItem(type, item_id, count);
        }
    }

    // Shop buy from Player
    public void BuyItem(Inventory buying_from, ItemType type, int item_id, int price, int count = 1) {
        if (!buying_from.HasItem(type, item_id, count)) {
            return;
        }
        if (inventory.TrySpendGold((int)(price * buy_rate) * count)) {
            buying_from.AddGold((int)(price * buy_rate) * count);
            buying_from.RemoveItem(type, item_id, count);

            inventory.AddItem(type, item_id, count);
        }
    }

    public void Load(ShopData data) {
        time_created = data.time_shop_created;
        inventory.LoadInventory(data.inventory_data);
    }

    public ShopData Save() {
        return new ShopData(this);
    }

    void Awake() {
        inventory = GetComponent<Inventory>();
        time_open = new TimeSpan(hours_open, minutes_open, 0);
    }

    void Update() {
        if (AccountHolder.has_valid_account) {
            if (time_left < TimeSpan.Zero) {
                GetNewShop();
                AccountHolder.account.SetShopData(Save());
                on_gen_new_shop.Invoke();
            }
        }
    }

    void Start() {
        if (AccountHolder.has_valid_account) {
            if (DateTime.Now - AccountHolder.account.shop.time_shop_created > time_open) {
                // If shop is expired Generate new shop
                GetNewShop();
                AccountHolder.account.SetShopData(Save());
            } else {
                // else load one on file
                Load(AccountHolder.account.shop);
            }
        }
    }

    void GetNewShop() {
        time_created = DateTime.Today;
        if (DateTime.Now.Hour >= 12) {
            time_created = time_created.AddHours(12);
        }


        InventoryData new_inventory = new InventoryData(UnityEngine.Random.Range(1000, 2001));

        new_inventory.base_inventory_data.base_pieces = new int[Database.GetMaxID(ItemType.BasePiece)];
        for (int i = 1; i < Database.GetMaxID(ItemType.BasePiece); i++) {
            if (i != Database.base_pieces.entrance_id && i != Database.base_pieces.exit_id) {
                new_inventory.base_inventory_data.base_pieces[i] = UnityEngine.Random.Range(0,11);
            }
        }
        new_inventory.base_inventory_data.enemy_groups = new int[Database.GetMaxID(ItemType.EnemyGroup)];
        for (int i = 1; i < Database.GetMaxID(ItemType.EnemyGroup); i++) {
            new_inventory.base_inventory_data.enemy_groups[i] = UnityEngine.Random.Range(0, 11);
        }

        inventory.LoadInventory(new_inventory);
    }
}

[Serializable]
public class ShopData {
    public InventoryData inventory_data;
    public DateTime time_shop_created;

    public ShopData(Shop s) {
        inventory_data = s.inventory.SaveInventory();
        time_shop_created = s.time_created;
    }

    public ShopData() {
        inventory_data = new InventoryData(0);
        time_shop_created = DateTime.MinValue;
    }
}
