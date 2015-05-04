using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputController : MonoBehaviour
{
	private PlayerController playerController;
	private bool jump;
	
	void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}
	
	void Update ()
	{
		// Read the jump input in Update so button presses aren't missed.
		//if (Input.GetButtonDown("Jump")) jump = true;

		print ("jump :" + jump);
	}
	
	void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = Input.GetAxis("Horizontal");
		
		// Pass all parameters to the character control script.
		playerController.Move( h, crouch , jump );
		
		// Reset the jump input once it has been used.
		jump = false;
	}
}