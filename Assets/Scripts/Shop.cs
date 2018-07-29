using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Shop : MonoBehaviour {

    [SerializeField][Range(0f,1f)] float _buy_rate;

    public Inventory inventory { get; private set; }
    public float buy_rate { get { return _buy_rate; } }

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

    void Awake() {
        inventory = GetComponent<Inventory>();
    }
}
