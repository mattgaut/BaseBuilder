using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonSelectionScreenUI : MonoBehaviour {

    [SerializeField] List<DisplayDungeon> dungeon_displays;
    [SerializeField] Button embark_button;

    BaseData[] dungeon_data;

	// Use this for initialization
	void Start () {
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Bases/", "*.bi");

        List<BaseData> possible_bases = new List<BaseData>();
        foreach (string file in files) {
            BaseData data = Load(file);
            if (data.map_valid) {
                possible_bases.Add(data);
            }
        }

        dungeon_data = new BaseData[dungeon_displays.Count];
        for (int i = 0; i < dungeon_displays.Count && possible_bases.Count > 0; i++) {
            int index = Random.Range(0, possible_bases.Count);
            dungeon_data[i] = possible_bases[index];
            dungeon_displays[i].Display(dungeon_data[i]);
            possible_bases.RemoveAt(index);
        }

        embark_button.onClick.AddListener(() => Embark());
    }

    private void Update() {
        if (!embark_button.interactable) {
            foreach (DisplayDungeon display in dungeon_displays) {
                if (display.selected) {
                    embark_button.interactable = true;
                }
            }
        }
    }

    BaseData Load(string filename) {
        if (!File.Exists(filename)) {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filename, FileMode.Open);

        BaseData bd = (BaseData)bf.Deserialize(file);

        file.Close();

        return bd;
    }

    void Embark() {
        for (int i = 0; i < dungeon_data.Length; i++) {
            if (dungeon_displays[i].selected) {
                SceneBridge.SetBaseData(dungeon_data[i]);

                SceneManager.LoadScene("BaseCrawlScene");
                break;
            }
        }
    }
}
