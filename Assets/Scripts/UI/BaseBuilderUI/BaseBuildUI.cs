using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseBuildUI : MonoBehaviour {

    [SerializeField] Toggle place_button, link_button, group_button;
    [SerializeField] BaseBuildManager builder;
    [SerializeField] BaseSaveLoadManager save_load_manager;

    [SerializeField] RectTransform right_side_panel;
    [SerializeField] Button right_side_panel_button;

    [SerializeField] GameObject pause_screen, saving_screen, loading_screen;
    [SerializeField] Button save_button, load_button, set_home_button, return_button, confirm_save_button, quit_button;
    [SerializeField] InputField save_field;
    [SerializeField] Button load_button_prefab;
    [SerializeField] RectTransform load_button_holder;

    bool right_side_panel_hidden;
    bool paused;
    bool saving, loading;

    void Awake() {
        place_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.PlaceBlocks); });
        link_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.Link); });
        group_button.onValueChanged.AddListener((value) => { if (value) builder.SwitchMode(BaseBuildManager.BuildMode.PlaceEnemies); });

        right_side_panel_button.onClick.AddListener(() => ToggleRightSidePanel());

        quit_button.onClick.AddListener(() => Time.timeScale = 1f);

        if (builder.free_build) {
            set_home_button.gameObject.SetActive(false);
            return_button.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        } else {
            if (AccountHolder.has_valid_account) {
                set_home_button.onClick.AddListener(() => SetHomeBase());
            } else {
                set_home_button.gameObject.SetActive(false);
            }
            return_button.onClick.AddListener(() => SceneManager.LoadScene("HUBMenu"));
        }

        save_button.onClick.AddListener(() => StartSave());
        load_button.onClick.AddListener(() => StartLoad());

        confirm_save_button.onClick.AddListener(() => EndSave(save_field.text));

        right_side_panel_hidden = false;
    }

    void Update() {
        if (loading) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                EndLoad();
                return;
            }
        }
        if (saving) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                EndSave();
                return;
            }
            UpdateSave();
        }
        if (!builder.took_input) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePause();
            }
        }
    }

    void TogglePause() {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        pause_screen.SetActive(paused);
        set_home_button.GetComponentInChildren<Text>().text = "Set As Home Base";
    }

    void ToggleRightSidePanel() {
        right_side_panel_hidden = !right_side_panel_hidden;

        if (right_side_panel_hidden) {
            right_side_panel.position += new Vector3(right_side_panel.rect.width, 0, 0);
        } else {
            right_side_panel.position -= new Vector3(right_side_panel.rect.width, 0, 0);
        }
    }

    void SetHomeBase() {
        if (!builder.map_valid) {
            set_home_button.GetComponentInChildren<Text>().text = "Map Not Valid";
            return;
        }
        if (AccountHolder.account.SetHomeBase(builder.Save())) {
            set_home_button.GetComponentInChildren<Text>().text = "Map Set";
        } else {
            set_home_button.GetComponentInChildren<Text>().text = "Not Enough Resources";
        }
    }

    void StartSave() {
        saving = true;
        saving_screen.SetActive(true);
    }

    void UpdateSave() {
        confirm_save_button.interactable = save_field.text != "";
    }

    void EndSave(string filename = "") {
        if (filename != "") {
            save_load_manager.Save(filename);
        }
        saving_screen.SetActive(false);
        saving = false;
    }

    void StartLoad() {
        loading_screen.SetActive(true);
        loading = true;
        RefreshBases();
    }

    void RefreshBases() {
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Bases/", "*.bi");

        for (int i = load_button_holder.childCount - 1; i >= 0; i--) {
            Destroy(load_button_holder.GetChild(i).gameObject);
        }

        foreach (string file in files) {
            Button new_button = Instantiate(load_button_prefab, load_button_holder);
            string[] filename_array = file.Split('/', '.');
            new_button.GetComponentInChildren<Text>().text = filename_array[filename_array.Length - 2];
            load_button_holder.sizeDelta += Vector2.up * new_button.GetComponent<RectTransform>().rect.height;
            new_button.onClick.AddListener(() => EndLoad(filename_array[filename_array.Length - 2]));
        }
    }

    void EndLoad(string filename = "") {
        if ("" != null) {
            builder.Load(save_load_manager.Load(filename));
        }
        loading_screen.SetActive(false);
        loading = false;
        TogglePause();
    }
}
