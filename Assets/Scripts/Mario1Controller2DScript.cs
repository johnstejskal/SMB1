using UnityEngine;
using System.Collections;

public class Mario1Controller2DScript : MonoBehaviour {

	public const string DIRECTION_RIGHT = "right";
	public const string DIRECTION_LEFT = "left";

	public LayerMask GroundLayer;
	float groundedRadius = .1f;		// Radius of the overlap circle to determine if grounded

	//Movement related variables
	public float moveSpeed;  //Our general move speed. This is effected by our
	//InputManager > Horizontal button's Gravity and Sensitivity
	//Changing the Gravity/Sensitivty will in turn result in more loose or tighter control
	
	public float sprintMultiplier;   //How fast to multiply our speed by when sprinting
	public float sprintDelay;        //How long until our sprint kicks in
	private float sprintTimer;       //Used in calculating the sprint delay
	private bool jumpedDuringSprint; //Used to see if we jumped during our sprint
	float hSpeed = 0;
	bool facingRight = true;

	//Jump related variables
	public float initialJumpForce;       //How much force to give to our initial jump
	public float extraJumpForce;         //How much extra force to give to our jump when the button is held down
	public float maxExtraJumpTime;       //Maximum amount of time the jump button can be held down for
	public float delayToExtraJumpForce;  //Delay in how long before the extra force is added
	private float jumpTimer;             //Used in calculating the extra jump delay
	private bool playerJumped;           //Tell us if the player has jumped
	private bool playerJumping;          //Tell us if the player is holding down the jump button
	public Transform groundChecker;      //Gameobject required, placed where you wish "ground" to be detected from

	Transform m_GroundCheckL;	
	Transform m_GroundCheckR;	
	Animator m_Animator;
	Rigidbody2D m_rigidbody2D;

	void Start()
	{

		m_GroundCheckL = transform.FindChild("GroundCheckL");
		m_GroundCheckR = transform.FindChild("GroundCheckR");	
		m_rigidbody2D = GetComponent<Rigidbody2D> ();
		m_Animator = GetComponent<Animator>();
	}
	private bool isGrounded;             //Check to see if we are grounded
	
	void Update () {
		//Casts a line between our ground checker gameobject and our player
		//If the floor is between us and the groundchecker, this makes "isGrounded" true
		if (Physics2D.Linecast (transform.position, m_GroundCheckL.position, 1 << LayerMask.NameToLayer ("Ground")) ||
			Physics2D.Linecast (transform.position, m_GroundCheckR.position, 1 << LayerMask.NameToLayer ("Ground"))) {
			isGrounded = true;
		} else
			isGrounded = false;
		/*
		if (Physics2D.OverlapCircle (m_GroundCheckL.position, groundedRadius, GroundLayer) || Physics2D.OverlapCircle (m_GroundCheckR.position, groundedRadius, GroundLayer) && m_rigidbody2D.velocity.y <= 0) {
			isGrounded = true;
		} else if (m_rigidbody2D.velocity.y > 0) {
			isGrounded = false;
			
		}
*/

		Debug.Log ("isGrounded:"+isGrounded);

		hSpeed = Input.GetAxis("Horizontal");

		m_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));
		m_Animator.SetBool ("IsGrounded", isGrounded);


		//flip player dependant on direction
		if(hSpeed > 0 && !facingRight)
			Flip();
		else if(hSpeed < 0 && facingRight)
			Flip();
		
		m_Animator.SetFloat ("VelocityY", m_rigidbody2D.velocity.y);


		//If our player hit the jump key, then it's true that we jumped!
		if (Input.GetButtonDown("Jump") && isGrounded){
			playerJumped = true;   //Our player jumped!
			playerJumping = true;  //Our player is jumping!
			jumpTimer = Time.time; //Set the time at which we jumped

		}
		
		//If our player lets go of the Jump button OR if our jump was held down to the maximum amount...
		if (Input.GetButtonUp("Jump") || Time.time - jumpTimer > maxExtraJumpTime){
			playerJumping = false; //... then set PlayerJumping to false
		}
		
		//If our player hit a horizontal key...
		if (Input.GetButtonDown("Horizontal")){
			sprintTimer = Time.time;  //.. reset the sprintTimer variable
			jumpedDuringSprint = false;  //... change Jumped During Sprint to false, as we lost momentum
		}
	}
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void FixedUpdate (){

		if (Input.GetButton ("Sprint")) {

		}
		//If our player is holding the sprint button, we've held down the button for a while, and we're grounded...
		//OR our player jumped while we were already sprinting...
		if (Input.GetButton("Sprint") && Time.time - sprintTimer > sprintDelay && isGrounded || jumpedDuringSprint){
			//... then sprint
			GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * sprintMultiplier,GetComponent<Rigidbody2D>().velocity.y);

			//If our player jumped during our sprint...
			if (playerJumped){
				jumpedDuringSprint = true; //... tell the game that we jumped during our sprint!
				//This is a tricky one. Basically, if we are already sprinting and our player jumps, we want them to hold their
				//momentum. Since they are no longer grounded, we would not longer return true on a regular sprint because
				//the build-up of sprint requires the player to be grounded. Likewise, if our player presses another horizontal
				//key, the jumpedDuringSprint would be set to false in our Update() function, thus causing a "loss" in momentum.
			}
		}
		else{
			//If we're not sprinting, then give us our general momentum
			GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime,GetComponent<Rigidbody2D>().velocity.y);
		}

		//GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime,GetComponent<Rigidbody2D>().velocity.y);

		//If our player pressed the jump key...
		if (playerJumped){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,initialJumpForce)); //"Jump" our player up in the air!
			playerJumped = false; //Our player already jumped, so no need to jump again just yet
		}
		
		//If our player is holding the jump button and a little bit of time has passed...
		if (playerJumping && Time.time - jumpTimer > delayToExtraJumpForce){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,extraJumpForce)); //... then add some additional force to the jump
		}
	}

	public void addDownForce(int force)
	{
		m_rigidbody2D.AddForce(new Vector2(0, force));
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		
		if (coll.gameObject.tag == "Enemy") {
			if(transform.position.y > coll.transform.position.y)
			m_rigidbody2D.AddForce(new Vector2(0, 800));
		}
	}
}