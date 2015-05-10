using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour {
	
	// Use this for initialization
	Vector3 direction;
	bool movingLeft = true;
	bool isDead = false;
	Vector3 currPosition;

	void Start () {

		currPosition = transform.position;
		direction = Vector3.left;
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
	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag == "Player") {
			if(coll.transform.position.y > transform.position.y)
			doSquash();
		}
	}

	void doSquash ()
	{
		if (isDead)
		return;

		isDead = true;

		GameObject.Destroy ( gameObject ) ;
	}
}
