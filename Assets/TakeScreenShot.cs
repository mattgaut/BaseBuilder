using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeScreenShot : MonoBehaviour {

    [SerializeField] RenderTexture cam_texture;

    public Texture2D GetScreenShot() {
        Texture2D capture = new Texture2D(cam_texture.width, cam_texture.height, TextureFormat.RGB24, false);

        RenderTexture.active = cam_texture;
        capture.ReadPixels(new Rect(0, 0, cam_texture.width, cam_texture.height), 0, 0);
        capture.Apply();

        return capture;
	}
}
