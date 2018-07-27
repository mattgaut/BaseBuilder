using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class AccountMenu : MonoBehaviour {

    [SerializeField] Button play_button;

    [SerializeField] LoginButton login_button;
    [SerializeField] Button create_account_button;
    [SerializeField] GameObject menu_panel, list_panel, create_panel;
    [SerializeField] InputField account_name_field;
    [SerializeField] Text error_text;

    [SerializeField] RectTransform button_holder;
    [SerializeField] Button button_prefab;

    void Start() {
        login_button.button.onClick.AddListener(() => OpenMenu());
        create_account_button.onClick.AddListener(() => CreateAndLoadAccount());

        if (AccountHolder.account.account_loaded) {
            login_button.SetAccount(AccountHolder.account.account_name);
            play_button.interactable = true;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseMenu();
        }    
    }

    void OpenMenu() {
        menu_panel.SetActive(true);
        RefreshAccounts();
    }

    void CloseMenu() {
        menu_panel.SetActive(false);
        list_panel.SetActive(true);
        create_panel.SetActive(false);
    }

    void CreateAndLoadAccount() {
        AccountData data = new AccountData(account_name_field.text);
        data.base_inventory_data = BaseInventoryData.empty;

        Directory.CreateDirectory(Application.persistentDataPath + "/Accounts/");
        if (File.Exists(Application.persistentDataPath + "/Accounts/" + data.account_name + ".ai")) {
            error_text.text = "Account Already Exists.";
            return;
        }

        FileStream file = File.Open(Application.persistentDataPath + "/Accounts/" + data.account_name + ".ai", FileMode.CreateNew);
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(file, data);

        file.Close();

        LoadAccount(data.account_name);

        error_text.text = "";
    }

    void RefreshAccounts() {
        for (int i = button_holder.childCount - 1; i >= 0; i--) {
            Destroy(button_holder.GetChild(i).gameObject);
        }
        button_holder.sizeDelta = new Vector2(button_holder.sizeDelta.x, 0);

        string[] files = Directory.GetFiles(Application.persistentDataPath + "/Accounts/", "*.ai");

        foreach (string file in files) {
            Button new_button = Instantiate(button_prefab, button_holder);
            string[] filename_array = file.Split('/', '.');
            new_button.GetComponentInChildren<Text>().text = filename_array[filename_array.Length - 2];
            button_holder.sizeDelta += Vector2.up * (new_button.GetComponent<RectTransform>().rect.height);
            new_button.onClick.AddListener(() => LoadAccount(filename_array[filename_array.Length - 2]));
        }
        button_holder.sizeDelta += Vector2.up * button_holder.GetComponent<VerticalLayoutGroup>().spacing * (files.Length - 1);
    }

    void LoadAccount(string account_name) {
        if (!File.Exists(Application.persistentDataPath + "/Accounts/" + account_name + ".ai")) {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Accounts/" + account_name + ".ai", FileMode.Open);

        AccountHolder.account.Load((AccountData)bf.Deserialize(file));
        file.Close();

        login_button.SetAccount(AccountHolder.account.account_name);
        play_button.interactable = true;
        CloseMenu();
    }

}
