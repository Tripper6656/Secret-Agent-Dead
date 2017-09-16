/** Secret Agent Dead
  * Collin Vossman and Tim Ripper
  *
  * Player Script for movement.
  * Adapted from: Unity 5 for beginners: 2D Platformer - running animation by inScope Studios on YouTube.
  **/


// Remember to remake the animation for new smoking.

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    [SerializeField] //Adds box in Inspector to edit movementSpeed from Unity.
    private float movementSpeed;

    private bool facingRight;
    //private bool attack;  //Use for attack

    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;

    [SerializeField]
    private bool airControl;

    private bool jump;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float jumpTime;

	private int timeIdle;  // IDLE ANIMATIONS

	void Start ()
	{
        timeIdle = 0;
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void FixedUpdate () //FixedUpdate limits the frame rate regardless of computer frame rate.
	{
		float horizontal = Input.GetAxis("Horizontal");  //Can check input settings in Unity from Edit->Project Settings->Input
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
		Flip(horizontal);



		if(timeIdle > 1000)  //Setup for idle animations
		{
            timeIdle = 0;
		}
		timeIdle++;
        resetValues();


	}

    IEnumerator Jumping()
    {
        yield return new WaitForSeconds(jumpTime);
        myAnimator.SetBool("jump", false);
    }


    private void Update()
    {
        HandleInput();
    }

    /** Moves and animates character during movement.
	  *
	  * @param horizontal : Horizontal input from keyboard
	  * @return void
	  */
    private void HandleMovement(float horizontal) 
	{
		myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));  //Make sure "speed" matches the parameter name for the condition to transition between animations.
        if (isGrounded && jump)
        {
            isGrounded = false;
            myAnimator.SetBool("jump", true);
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            StartCoroutine(Jumping());
        }

		timeIdle = 0;
	}


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jump = true;
        }
    }

	/** Flips sprite to face the direction it is moving. 
	  *
	  * @param horizontal : Horizontal input from keyboard
	  * @return void
	  */
	private void Flip(float horizontal) //Flips to appropriate direction
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}	
	}
    /*

	private void HandleIdle() //Remember to set exit time for idle animations so they automatically go back to regular idle.
	{
		myAnimator.SetBool("timeIdle", true);  //could make timeIdle a bool and just set to true
		timeIdle = 0; //may need 0.0 if float is required for timeIdle in animation
		//myAnimator.SetBool("timeIdle", false); 
	}
    */
    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <=0) //If we are falling down (y < 0) or not moving (y = 0)
        {
            foreach(Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void resetValues ()
    {
        jump = false;
    }
}