using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class BaseSaveLoadManager : MonoBehaviour {

    [SerializeField] BaseBuildManager base_manager;
    [SerializeField] Text filename;

    public void Save() {
        BaseData data = base_manager.Save();

        BinaryFormatter bf = new BinaryFormatter();

        Directory.CreateDirectory(Application.persistentDataPath + "/Bases/");
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + filename.text + ".bi", FileMode.OpenOrCreate);

        bf.Serialize(file, data);

        file.Close();
    }

    public void Save(string filename) {
        BaseData data = base_manager.Save();

        BinaryFormatter bf = new BinaryFormatter();

        Directory.CreateDirectory(Application.persistentDataPath + "/Bases/");
        Debug.Log(Application.persistentDataPath + "/Bases/");
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + filename + ".bi", FileMode.OpenOrCreate);

        bf.Serialize(file, data);

        file.Close();
    }

    public void Load() {
        if (!File.Exists(Application.persistentDataPath + "/Bases/" + filename.text + ".bi")) {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + filename.text + ".bi", FileMode.Open);

        base_manager.Load((BaseData)bf.Deserialize(file));

        file.Close();
    }

    public BaseData Load(string filename) {;
        if (!File.Exists(Application.persistentDataPath + "/Bases/" + filename + ".bi")) {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + filename + ".bi", FileMode.Open);

        BaseData bd = (BaseData)bf.Deserialize(file);

        file.Close();

        return bd;
    }
}
