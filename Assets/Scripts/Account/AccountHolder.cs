using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(Account))]
public class AccountHolder : MonoBehaviour {

    static AccountHolder instance;
    static bool has_valid_account { get { return instance != null && instance.account.account_loaded; } }

    Account account;

    public static Account GetAccount() {
        if (instance == null) {
            return null;
        }
        return instance.account;
    }

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        } else {
            Destroy(gameObject);
        }
    }

    void Init() {
        account = GetComponent<Account>();
    }

    void SaveCurrentAccount() {
        if (!has_valid_account) {
            return;
        }
        AccountData data = account.Save();

        Directory.CreateDirectory(Application.persistentDataPath + "/Accounts/");
        FileStream file = File.Open(Application.persistentDataPath + "/Accounts/" + data.account_name + ".ai", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(file, data);

        file.Close();
    }

    private void OnApplicationQuit() {       
        SaveCurrentAccount();
    }
}
