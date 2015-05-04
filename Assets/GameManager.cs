using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	void awake(){
		Application.runInBackground = true;

		//dont destroy the game manager between scenes
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
