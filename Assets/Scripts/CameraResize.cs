using UnityEngine;
using System.Collections;

public class CameraResize : MonoBehaviour {

	private float s_baseOrthographicSize;
	[SerializeField]int pixelToUnit;
	// Use this for initialization
	void Start () {
		print (this + "Start:"+pixelToUnit);
		s_baseOrthographicSize = Screen.height / pixelToUnit / 2.0f;
		Camera.main.orthographicSize = s_baseOrthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
