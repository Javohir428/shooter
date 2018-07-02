using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {
    //name of the scene you want to load
    public string scene;
	public Color loadToColor = Color.white;
	
	public void GoFade()
    {
        Initiate.Fade(scene, loadToColor, 1.0f);
    }
}
