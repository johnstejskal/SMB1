using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public const string DIRECTION_RIGHT = "right";
	public const string DIRECTION_LEFT = "left";
	public const float BASE_MOVE_SPEED = 10f;

	public LayerMask GroundLayer;

	bool isJumpDown = false;

	bool isInitialJump {
		get;
		set;
	}

	float hSpeed = 0;
	bool isGrounded = false;
	bool facingRight = true;
	bool isJumpAllowed {
		get;
		set;
	}

	

	float runSpeed = 5;
	float moveSpeed;
	float jumpSpeed = 0.2f;
	float jumpCount = 1;

	string currentDirection;
	public float holdJumpUpwardSpeed = 5.0f; // speed at which object moves up towards max
	public float holdJumpMaxHeight = 10.0f;  // max hold jump height before applying gravity
	private float initialYPosition = 0;      // initial y position used to calculate at if at max
	private float currentYPosition = 0;    
	

	[SerializeField] float maxSpeed = 1f;
	
	private GameObject gm;

	float _currSpeed {get;set;}
	
	// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 600f;			// Amount of force added when the player jumps.	
	
	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = true;			// Whether or not a player can steer while jumping;



	Transform m_GroundCheckL;	
	Transform m_GroundCheckR;							
	
	float groundedRadius = .1f;							// Radius of the overlap circle to determine if grounded

	Transform ceilingCheck;								// A position marking where to check for ceilings

	Animator m_Animator;
	Rigidbody2D m_rigidbody2D;
	

	
	private float sin45 = Mathf.Sin(45); // get sine(45) value

	void Start()
	{
		isJumpAllowed = false;
		currentDirection = DIRECTION_RIGHT;
		gm = GameObject.Find ("_GM");
		
		moveSpeed = BASE_MOVE_SPEED;
		m_GroundCheckL = transform.FindChild("GroundCheckL");
		m_GroundCheckR = transform.FindChild("GroundCheckR");
		
		ceilingCheck = transform.Find("CeilingCheck");
		m_Animator = GetComponent<Animator>();
		m_rigidbody2D = GetComponent<Rigidbody2D> ();

		_currSpeed = BASE_MOVE_SPEED;

	}
	


	void Update(){
	
		/*
		if (Input.GetButtonDown ("Jump")) {
			if(isGrounded){
			
			}
			else
			{
				m_rigidbody2D.velocity = new Vector2(0, 0);
			}
			m_rigidbody2D.AddForce (Vector2.up * (1500 * Time.deltaTime), ForceMode2D.Impulse);
		}*/


		if (Input.GetButton ("Jump"))
			isJumpDown = true;
		else
			isJumpDown = false;

		hSpeed = Input.GetAxis("Horizontal");


	}

	void FixedUpdate()
	{

		m_rigidbody2D.velocity = new Vector2 (hSpeed * moveSpeed, m_rigidbody2D.velocity.y);


		//check ground
		if (Physics2D.OverlapCircle (m_GroundCheckL.position, groundedRadius, GroundLayer) || Physics2D.OverlapCircle (m_GroundCheckR.position, groundedRadius, GroundLayer) && m_rigidbody2D.velocity.y <= 0) {
			isGrounded = true;
			isJumpAllowed = true;
			isInitialJump = true;
			initialYPosition = m_rigidbody2D.position.y;
		} else if (m_rigidbody2D.velocity.y > 0) {
			isGrounded = false;

		}

		m_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));
		m_Animator.SetBool ("IsGrounded", isGrounded);


		

		//flip player dependant on direction
		if(hSpeed > 0 && !facingRight)
		Flip();
		else if(hSpeed < 0 && facingRight)
		Flip();

		m_Animator.SetFloat ("VelocityY", m_rigidbody2D.velocity.y);

		currentYPosition = m_rigidbody2D.position.y;

		//put the jump in the late 
		if (isJumpDown) {

			//if just left the ground

				//if(currentYPosition - initialYPosition < 2)
				//{
					


					if(m_rigidbody2D.velocity.y >= 30 && isJumpAllowed)
						isJumpAllowed = false;
					
					//if(isJumpAllowed)
						//m_rigidbody2D.AddForce (Vector2.up * (300 * Time.deltaTime), ForceMode2D.Impulse);


			//while(m_rigidbody2D.velocity.y < 10)
			//{
				//if(isJumpAllowed)
				m_rigidbody2D.AddForce (Vector2.up * (300 * Time.deltaTime), ForceMode2D.Impulse);
			//}



				//}








		} else {
			//Debug.Log ("JUMP NOT DOWN");
		}
	} 
	
	void LateUpdate(){





	}
	



	
	public void Move(float move, bool crouch, bool jump)
	{
		return;
		if (isGrounded)
		jumpCount = 0;

		moveSpeed = move * 10f;
		if(isGrounded || airControl)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(move));
			
			// Move the character
			GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
			
			

		}
		
		// If the player should jump...
		if ((isGrounded || jumpCount < 1) && jump)
		{	jumpCount ++;

			if(!isGrounded)
			GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);

			//anim.SetBool("Ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

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
}
