using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplaySavedBases : MonoBehaviour {

    [SerializeField] RectTransform content;
    [SerializeField] Button button_prefab;

    [SerializeField] BaseSaveLoadManager loader;

    [SerializeField] string scene_to_load;

    bool selected = false;

	void Start () {
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Bases/", "*.bi");

        foreach (string file in files) {
            Button new_button = Instantiate(button_prefab, content);
            string[] filename_array = file.Split('/', '.');
            new_button.GetComponentInChildren<Text>().text = filename_array[filename_array.Length - 2];
            content.sizeDelta += Vector2.up * new_button.GetComponent<RectTransform>().rect.height;
            new_button.onClick.AddListener(() => Load(filename_array[filename_array.Length - 2]));
        }
    }

    void Load(string filename) {
        if (!selected) {
            selected = true;

            BaseData data = loader.Load(filename);
            if (data == null) {
                selected = false;
                return;
            }

            SceneBridge.SetBaseData(data);

            SceneManager.LoadScene(scene_to_load);
        }
    }


}
