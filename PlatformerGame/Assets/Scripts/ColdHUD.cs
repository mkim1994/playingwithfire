using UnityEngine;
using System.Collections;

public class ColdHUD : MonoBehaviour {

	public Transform child;
	public Transform reindeer;
	public float coldDistance = 100;
	private float distance;
	public float freezeRange = 15; //how far you can walk while freezing


	// Use this for initialization
	void Start ()
	{
		// Set the texture so that it is the the size of the screen and covers it.
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		Color textureColor = guiTexture.color;
		textureColor.a = 0;
		guiTexture.color = textureColor;
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance (child.position, reindeer.position);
		if (distance > coldDistance){
			float coldness = (distance - coldDistance)/freezeRange; // how far away the child is
			if (coldness > 0.6f){ Application.LoadLevel(Application.loadedLevelName);} //restart level }
			Debug.Log(coldness);
			Color textureColor = guiTexture.color;
			textureColor.a = coldness;
			guiTexture.color = textureColor;
		}
	}
}

