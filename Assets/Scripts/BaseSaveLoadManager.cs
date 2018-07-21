using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(IBaseSaveLoad))]
public class BaseSaveLoadManager : MonoBehaviour {

    IBaseSaveLoad save_load_object;

    string selected_filename = "";

    [SerializeField] bool hide_gui; 

    private void Awake() {
        save_load_object = GetComponent<IBaseSaveLoad>();
    }

    private void OnGUI() {
        if (!hide_gui) {
            selected_filename = GUI.TextField(new Rect(0, 0, 100, 20), selected_filename);
            selected_filename = new String(selected_filename.Where(Char.IsLetterOrDigit).ToArray());
            if (GUI.Button(new Rect(0, 20, 100, 20), "Save")) {
                Save();
            }
            if (GUI.Button(new Rect(0, 40, 100, 20), "Load")) {
                Load();
            }
        }
    }

    public void Save() {
        BaseData data = save_load_object.Save();

        BinaryFormatter bf = new BinaryFormatter();

        Directory.CreateDirectory(Application.persistentDataPath + "/Bases/");
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + selected_filename + ".bi", FileMode.OpenOrCreate);

        bf.Serialize(file, data);

        file.Close();
    }

    public void Save(string filename) {
        BaseData data = save_load_object.Save();

        BinaryFormatter bf = new BinaryFormatter();

        Directory.CreateDirectory(Application.persistentDataPath + "/Bases/");
        Debug.Log(Application.persistentDataPath + "/Bases/");
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + filename + ".bi", FileMode.OpenOrCreate);

        bf.Serialize(file, data);

        file.Close();
    }

    public void Load() {
        if (!File.Exists(Application.persistentDataPath + "/Bases/" + selected_filename + ".bi")) {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Bases/" + selected_filename + ".bi", FileMode.Open);

        save_load_object.Load((BaseData)bf.Deserialize(file));

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
