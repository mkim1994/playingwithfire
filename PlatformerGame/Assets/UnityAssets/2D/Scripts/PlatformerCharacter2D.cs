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
		[Range(0, 1)] [SerializeField] private float mountSpeed = 3.0f;

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsBashable; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsClimbable; // A mask determining what the character can climb

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private float maxClimbSpeed = 2f;
		public Vector3 spawnPoint = new Vector3(0,0,1);

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
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
		}

		public void Dismount(){
			anim.SetBool ("Mount", false);
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

        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                    crouch = true;
            }

			if (anim.GetBool("Mount")) {crouch = false;} //cant croutch while mounted

            // Set whether or not the character is crouching in the animator
            anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*crouchSpeed : move);

				move = (anim.GetBool("Grab") ? move*grabSpeed : move); //slow down while grabbing obj

				move = (anim.GetBool("Mount") ? move*mountSpeed : move); //speed up on raindeer

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                rigidbody2D.velocity = new Vector2(move*maxSpeed, rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                    // ... flip the player.
                    Flip();
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                    // ... flip the player.
                    Flip();
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