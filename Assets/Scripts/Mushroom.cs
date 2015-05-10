using UnityEngine;
using System.Collections;

public class Mushroom : MonoBehaviour {

	Vector3 direction;
	bool movingLeft = false;
	bool isDead = false;
	Vector3 currPosition;
	
	void Start () {
		
		currPosition = transform.position;
		direction = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate (direction * 2 * Time.deltaTime);
		//Debug.Log (transform.position.x +" "+currPosition.x);
		
		
		if (transform.position.x > currPosition.x && movingLeft) 
		{
			movingLeft = false;
			direction = Vector3.right;
		}
		else if (transform.position.x < currPosition.x && !movingLeft)
		{
			movingLeft = true;
			direction = Vector3.left;
		}
		
		
		currPosition = transform.position;
		
		
		
	}
}
