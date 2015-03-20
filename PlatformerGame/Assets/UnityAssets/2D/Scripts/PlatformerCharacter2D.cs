using UnityEngine;

namespace UnitySampleAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        private bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 300f; // Amount of force added when the player jumps.	
		[SerializeField] private float mountJumpForce = 6000f; // Amount of force added when the raindeer jumps.	

        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, 1)] [SerializeField] private float grabSpeed = .66f;
		[Range(0, 1)] [SerializeField] private float digSpeed = .66f;
		[Range(0, 1)] [SerializeField] private float mountSpeed = 3.0f;

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsBashable; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsClimbable; // A mask determining what the character can climb
		[SerializeField] private LayerMask whatIsDigable; // A mask determining what the character can dig

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private float maxClimbSpeed = 2f;
		public Vector3 spawnPoint = new Vector3(0,0,1);

		private bool rabbitAspect;

		private bool digging;

		void OnCollisionEnter2D(Collision2D collision)
		{
			Debug.Log("Collision!\n");
			if (collision.collider.tag == "Diggable")
			{
				Debug.Log("Collided with a diggable!\n");
			}

			if (collision.gameObject.tag == "DeadAnimal"){
				if (Input.GetKey(KeyCode.LeftControl)){
					rabbitAspect = true;
				}
			}
		}

		void OnCollisionExit2D(Collision2D collision)
		{
			Debug.Log("Exit Collision!\n");
			if (collision.collider.tag == "Diggable")
			{
				Debug.Log("Exiting collision with a diggable!\n");
			}
		}

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();

            digging = false;

			anim.SetBool ("Reindeer",false);

			//check if you have the ability to dig from the beginning of the level
			if(Application.loadedLevelName == "01"){
				rabbitAspect = false;
			}
			else{
				rabbitAspect = true;
			}
        }

        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround) || 
				Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable);

            anim.SetBool("Ground", grounded);

            // Set the vertical animation
            anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
        }

		public void Respawn(){
			transform.position = spawnPoint;
		}

		public void Mount(Vector3 rPos){
			transform.position = rPos;//center at raindeer pos
			anim.SetBool ("Mount", true);
			anim.SetBool ("Reindeer",true);
		}

		public void Dismount(){
			anim.SetBool ("Mount", false);
			anim.SetBool ("Reindeer",false);
		}

		public void Climb (){
			if (Physics2D.OverlapCircle (groundCheck.position, groundedRadius*2, whatIsClimbable)) {
				anim.SetBool ("Climb", true);
					if (rigidbody2D.velocity.y < maxClimbSpeed && !anim.GetBool("Mount")){
						rigidbody2D.AddForce(new Vector2(0f, 50f));
					}
				} 
			else { 
				anim.SetBool ("Climb", false);
				}
			}

		private GameObject breakingRock;
		public void Bash (GameObject rock){
			if (anim.GetBool("Mount") && grounded){
				anim.SetBool("Bash",true);
				breakingRock = rock;
				Invoke("BreakRock",1.0f); //change 1.0f to however long the animation takes
			}
		}

		public void BreakRock (){
			anim.SetBool ("Bash", false); //no longer bashing
			Destroy (breakingRock);
			}

        public void Move(float move_h, float move_v, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && (anim.GetBool("Crouch") || anim.GetBool("Dig")))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                    crouch = true;
            }
			
			if ((crouch || digging) && !anim.GetBool("Mount") && Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable)){
				if (!digging) Debug.Log("Start Digging\n");
				digging = true;
				// don't collide with diggable objects
				// STYLE NOTE: THERE SHOULD BE SOME WAY TO GET THE LAYER NUMBERS WITHOUT HARD CODING THEM
				// IF THE LAYER NUMBERS CHANGE, NEED TO CHANGE THESE TOO
				Physics2D.IgnoreLayerCollision(8, 11, true);		
			}
			else
			{
				if (digging) Debug.Log("Stop Digging\n");
				digging = false;
				Physics2D.IgnoreLayerCollision(8, 11, false);
			}

			//anim.SetBool ("Dig", digging); //show up in animation (need to add varible to it...)

			if (digging) 
			{
				// adjust speed while digging
				move_h *= digSpeed;
				move_v *= digSpeed;

				// animate at maximum speed (probably the same)
				anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(move_h), Mathf.Abs(move_v)));

				rigidbody2D.velocity = new Vector2(move_h * maxSpeed, move_v * maxSpeed);
			}
			else {	

				if (anim.GetBool("Mount")) {crouch = false;} //cant croutch while mounted

	            // Set whether or not the character is crouching in the animator
	            anim.SetBool("Crouch", crouch);

	            //only control the player if grounded or airControl is turned on
	            if (grounded || airControl)
	            {
	                // Reduce the speed if crouching by the crouchSpeed multiplier
	                move_h = (crouch ? move_h*crouchSpeed : move_h);

					move_h = (anim.GetBool("Grab") ? move_h*grabSpeed : move_h); //slow down while grabbing obj

					move_h = (anim.GetBool("Mount") ? move_h*mountSpeed : move_h); //speed up on raindeer

	                // The Speed animator parameter is set to the absolute value of the horizontal input.
	                anim.SetFloat("Speed", Mathf.Abs(move_h));

	                // Move_h the character
	                rigidbody2D.velocity = new Vector2(move_h*maxSpeed, rigidbody2D.velocity.y);
	            }
	            // If the player should jump...
	            if (grounded && jump && anim.GetBool("Ground") && !crouch)
	            {
	                // Add a vertical force to the player.
	                grounded = false;
	                anim.SetBool("Ground", false);
					if (rigidbody2D.velocity.y < maxClimbSpeed) //if not already climbing at max speed
					{
		                if (anim.GetBool("Mount")){
								rigidbody2D.AddForce(new Vector2(0f, mountJumpForce));
							}
						else {rigidbody2D.AddForce(new Vector2(0f, jumpForce));}
					}
	            }
			}
			// If the input is moving the player right and the player is facing left...
	        if (move_h > 0 && !facingRight)
	            // ... flip the player.
	        	Flip();
	        // Otherwise if the input is moving the player left and the player is facing right...
	        else if (move_h < 0 && facingRight)
	            // ... flip the player.
	            Flip();
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}