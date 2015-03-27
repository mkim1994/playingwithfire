using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public Canvas canvas;

	// Use this for initialization
	void Start () {
		//Application.LoadLevel ("Intro");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void nextLevel(){
		Invoke ("nextlvl", 0.6f);

	}

	void nextlvl(){
		Application.LoadLevel ("Intro");
	}
}
