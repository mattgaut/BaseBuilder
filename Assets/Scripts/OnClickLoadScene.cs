using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickLoadScene : MonoBehaviour {

    [SerializeField] Button button;
    [SerializeField] string to_load;

	// Use this for initialization
	void Awake() {
        button.onClick.AddListener(() => LoadScene());
    }

    void LoadScene() {
        SceneManager.LoadScene(to_load);
    }
}
