using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FreeBuildButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => LoadFreeBuild());
	}
	
	void LoadFreeBuild() {
        SceneBridge.SetFreeBuild(true);
        SceneManager.LoadScene("BaseBuildScene");
    }
}
