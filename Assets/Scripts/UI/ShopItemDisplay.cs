using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour {
    [SerializeField] Button plus_one, minus_one, confirm;
    [SerializeField] InputField transaction_count_input_field;

    [SerializeField] Text item_name, for_sale_count_text, price_text, confirm_text;
    [SerializeField] Image image;

    int for_sale_count, price, transaction_count;

    int price_limit;
    bool price_limit_set;

    public void SetName(string name) {
        item_name.text = name;
    }
    public void SetItemCount(int count) {
        if (count <= 0) {
            return;
        }
        for_sale_count_text.text = "x " + count;
        for_sale_count = count;

        SetTransactionCount(1);
    }

    public void SetPrice(int price) {
        if (price <= 0) {
            return;
        }
        this.price = price;
        price_text.text = (transaction_count * price) + " Gold";
    }
    public void SetPriceLimit(int limit) {
        price_limit = limit;
        price_limit_set = true;
        CheckUnderLimit();
    }
    public void SetConfirmText(string text) {
        confirm_text.text = text;
    }

    public void SetConfirmAction(UnityAction<int> action) {
        confirm.onClick.AddListener(() => action(transaction_count));
    }

    void Awake() {
        plus_one.onClick.AddListener(() => AddOne());
        minus_one.onClick.AddListener(() => SubtractOne());

        transaction_count_input_field.onEndEdit.AddListener((string new_text) => SetTransactionCount(new_text));
    }

    void AddOne() {
        SetTransactionCount(transaction_count + 1);
    }

    void SubtractOne() {
        SetTransactionCount(transaction_count - 1);
    }

    void SetTransactionCount(int count) {
        if (count <= 0) {
            return;
        }
        if (count > for_sale_count) {
            count = for_sale_count;
        }
        transaction_count = count;
        transaction_count_input_field.text = transaction_count + "";

        // Make sure buttons cant be clicked when they would have no effect
        minus_one.interactable = (count != 1);
        plus_one.interactable = (count != for_sale_count);

        price_text.text = (count * price) + " Gold";
        CheckUnderLimit();
    }

    void SetTransactionCount(string count_text) {
        SetTransactionCount(int.Parse(count_text));
    }

    void CheckUnderLimit() {
        confirm.interactable = (!price_limit_set || (price * transaction_count) <= price_limit);
    }

}
