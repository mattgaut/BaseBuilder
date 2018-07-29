using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(Account))]
public class AccountHolder : MonoBehaviour {

    public static bool has_valid_account { get { return instance != null && instance._account.account_loaded; } }
    public static Account account {
        get {
            if (instance == null) {
                return null;
            }
            return instance._account;
        }
    }

    static AccountHolder instance;

    Account _account;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
            Init();
        } else {
            Destroy(gameObject);
        }
    }

    void Init() {
        _account = GetComponent<Account>();
    }

    void SaveCurrentAccount() {
        if (!has_valid_account) {
            return;
        }
        AccountData data = _account.Save();

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
