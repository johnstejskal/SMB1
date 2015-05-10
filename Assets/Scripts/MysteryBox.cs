using UnityEngine;
using System.Collections;

public class MysteryBox : MonoBehaviour {

	public bool isBoxEmpty;
	Vector2 startPos;
	Mario1Controller2DScript playerController;
	Transform coinBoxSprite;
	GameObject emptyBoxSprite;
	

	// Use this for initialization
	void Start () {
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mario1Controller2DScript>();
		isBoxEmpty = false;
		startPos = this.transform.position;
		coinBoxSprite = transform.Find("CoinBox");
	}
	
	// Update is called once per frame
	void Update () {
		//Vector2 pos = Vector2.Lerp (startPos, new Vector2(startPos.x, startPos.y + 50), Time.fixedDeltaTime / 1000);
		//transform.position = new Vector3(pos.x, pos.y, transform.position.y);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if(coll.gameObject.tag == "HeadCollider")
		{
			if(!isBoxEmpty)
			{
				//playerController.addDownForce(-500);
				doBump();


			}
			else
			{

			}
		}
		
	}

	void doBump ()
	{
		isBoxEmpty = true;
		coinBoxSprite.position = new Vector3(startPos.x, startPos.y + .5f);
		StartCoroutine(CR_reposition(0.2F));
	}
	
	IEnumerator CR_reposition(float time)
	{
		yield return new WaitForSeconds(time);
		RepositionBox();

	}
	
	void RepositionBox()
	{
		emptyBoxSprite = Instantiate(Resources.Load("EmptyBox") as GameObject);
		GameObject item = Instantiate(Resources.Load("CoinBoxCoin") as GameObject);
		emptyBoxSprite.transform.position = startPos;
		coinBoxSprite.position = startPos;
		GameObject.Destroy ( coinBoxSprite.gameObject ) ;
		item.transform.position = new Vector2(startPos.x, startPos.y + 2);
	}


}
