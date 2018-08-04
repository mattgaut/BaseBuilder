using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShop : MonoBehaviour {

    [SerializeField] Shop shop;
    [SerializeField] Text gold_text, player_gold_text, shop_refresh_text;

    [SerializeField] RectTransform shop_base_pieces, shop_enemy_groups, player_base_pieces, player_enemy_groups;

    [SerializeField] ShopItemDisplay item_display_prefab;

    Account patron;

    public void RefreshInventories() {
        RefreshShopBasePieces();
        RefreshShopEnemyGroups();
        RefreshPlayerBasePieces();
        RefreshPlayerEnemyGroups();
    }

    void Awake() {
        patron = AccountHolder.account;

        RefreshInventories();
    }

    void Update() {
        RefreshDisplay();
    }

    void RefreshDisplay() {
        gold_text.text = "Shop Gold: " + shop.inventory.gold;
        shop_refresh_text.text = "Shop Refreshes In: " + shop.time_left.Hours.ToString("00") + ":" + shop.time_left.Minutes.ToString("00") + ":" + shop.time_left.Seconds.ToString("00");
        player_gold_text.text = "Your Gold: " + patron.inventory.gold;
    }

    void RefreshPlayerBasePieces() {
        RefreshItems(player_base_pieces, ItemType.BasePiece, patron.inventory);
    }

    void RefreshPlayerEnemyGroups() {
        RefreshItems(player_enemy_groups, ItemType.EnemyGroup, patron.inventory);
    }

    void RefreshShopBasePieces() {
        RefreshItems(shop_base_pieces, ItemType.BasePiece, shop.inventory);
    }

    void RefreshShopEnemyGroups() {
        RefreshItems(shop_enemy_groups, ItemType.EnemyGroup, shop.inventory);
    }

    void RefreshItems(RectTransform attach_to, ItemType item, Inventory load_from) {
        ClearChildren(attach_to);

        LoadItems(attach_to, item, load_from);
    }

    void LoadItems(RectTransform attach_to, ItemType item_type, Inventory load_from) {
        bool buying_from_shop = load_from == shop.inventory;
        for (int id = 0; id <= Database.GetMaxID(item_type); id++) {
            int item_count = load_from.GetItemCount(item_type, id);
            if (item_count > 0) {
                ShopItemDisplay new_display = Instantiate(item_display_prefab, attach_to);
                IItem item = Database.GetItem(item_type, id);
                LoadItemDisplay(new_display, Database.GetItem(item_type, id), item_count);
                new_display.SetPrice(buying_from_shop ? item.price : (int)(item.price * shop.buy_rate));
                new_display.SetPriceLimit(buying_from_shop ? patron.inventory.gold : shop.inventory.gold);
                new_display.SetConfirmText(buying_from_shop ? "Buy" : "Sell");
                int id_copy = id;
                if (buying_from_shop) {
                    new_display.SetConfirmAction((transaction_count) => SellItem(item_type, id_copy, item.price, transaction_count));
                } else {
                    new_display.SetConfirmAction((transaction_count) => BuyItem(item_type, id_copy, item.price, transaction_count));
                }
                attach_to.sizeDelta += Vector2.up * new_display.GetComponent<RectTransform>().rect.height;
            }
        }
    }

    void ClearChildren(RectTransform clear_from) {
        for (int i = clear_from.childCount - 1; i >= 0; i--) {
            Destroy(clear_from.GetChild(i).gameObject);
        }
        clear_from.sizeDelta = new Vector2(clear_from.sizeDelta.x, 0);
    }

    void LoadItemDisplay(ShopItemDisplay item_display, IItem item, int count) {
        item_display.SetName(item.item_name);
        item_display.SetItemCount(count);
    }

    void SellItem(ItemType type, int id, int price, int transaction_count) {
        shop.SellItem(patron.inventory, type, id, price, transaction_count);
        patron.SetShopData(shop.Save());
        RefreshInventories();
    }

    void BuyItem(ItemType type, int id, int price, int transaction_count) {
        shop.BuyItem(patron.inventory, type, id, price, transaction_count);
        patron.SetShopData(shop.Save());
        RefreshInventories();
    }
}
