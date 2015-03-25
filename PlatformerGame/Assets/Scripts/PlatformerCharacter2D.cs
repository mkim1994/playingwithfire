using UnityEngine;

namespace UnitySampleAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        public bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
		[SerializeField] private float maxHeight = 12f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 300f; // Amount of force added when the player jumps.	
		[SerializeField] private float mountJumpForce = 6000f; // Amount of force added when the raindeer jumps.	
		[SerializeField] private int maxFlaps = 3; // The fastest the player can travel in the x axis.


        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, 1)] [SerializeField] private float grabSpeed = .66f;
		[Range(0, 1)] [SerializeField] private float digSpeed = .66f;
		[Range(0, 1)] [SerializeField] private float mountSpeed = 3.0f;

        [SerializeField] private bool airControl = true; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsBashable; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsClimbable; // A mask determining what the character can climb
		[SerializeField] private LayerMask whatIsDigable; // A mask determining what the character can dig

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .4f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private float maxClimbSpeed = 2f;
		public Vector3 spawnPoint = new Vector3(0,0,1);
		public bool canFly = false; 

		private bool rabbitAspect;
		private int flaps;


		void OnCollisionEnter2D(Collision2D collision)
		{
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
			flaps = maxFlaps*10;

			anim.SetBool("Reindeer",false);
			anim.SetBool("Grab", false);
			anim.SetBool("Fly", false);
			anim.SetBool("Mount", false);

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
			grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround)
					|| Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable);
            anim.SetBool("Ground", grounded);
			if (grounded){flaps = maxFlaps*10;}

            // Set the vertical animation - TODO: FELIPE QUESTIONS THE EXISTENCE OF THIS LINE OF CODE
            anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
        }

		public void Respawn(){
			transform.position = spawnPoint;
		}

		public void Mount(Vector3 rPos){
			transform.position = rPos;//center at raindeer pos
			anim.SetBool("Mount", true);
			anim.SetBool("Reindeer",true);
		}

		public void Dismount(){
			anim.SetBool("Mount", false);
			anim.SetBool("Reindeer",false);
		}

		public void Climb (){
			if (Physics2D.OverlapCircle(groundCheck.position, groundedRadius*1.5f, whatIsClimbable)) 
			{
				anim.SetBool("Climb", true);
				if (rigidbody2D.velocity.y < maxClimbSpeed && !anim.GetBool("Mount")){
					rigidbody2D.AddForce(new Vector2(0f, 50f));
				}
			} 
			else 
			{ 
				anim.SetBool ("Climb", false);
			}
		}

		private GameObject breakingRock;
		public void Bash (GameObject rock){
			if (anim.GetBool("Mount") && grounded)
			{
				anim.SetBool("Bash",true);
				breakingRock = rock;
				Invoke("BreakRock",1.0f); //change 1.0f to however long the animation takes
			}
		}

		public void BreakRock (){
			anim.SetBool ("Bash", false); //no longer bashing
			Destroy (breakingRock);
			}

        public void Move(float move_h, float move_v, bool dig, bool jump)
        {			
        	// toggle digging when the controller says 'dig' only if not already digging, not mounted, and touching a tunnel
        	if (dig && !anim.GetBool("Dig") && !anim.GetBool("Mount") && Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable))
			{
				Debug.Log("Start Digging\n");
				Physics2D.IgnoreLayerCollision(8, 11, true);
				rigidbody2D.gravityScale = 0.0f;
				anim.SetBool("Dig", true);
				// sanity check
				anim.SetBool("Fly", false);
				transform.position = new Vector3(transform.position.x,transform.position.y-0.01f,0f);
			}
			else if (anim.GetBool("Dig"))
			{
				// while digging, check every tick to see if we exit the tunnel
				if ( !Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable) )
				{
					Debug.Log("Stop Digging\n");
					Physics2D.IgnoreLayerCollision(8, 11, false);
					rigidbody2D.gravityScale = 1.0f;
					anim.SetBool("Dig", false);
				}				
			}

			// handle movement while digging
			if (anim.GetBool("Dig")) 
			{
				Debug.Log(Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsDigable) );
				move_h *= digSpeed;
				move_v *= digSpeed;
				anim.SetFloat("Speed", Mathf.Abs(move_h));
				anim.SetFloat("vSpeed", Mathf.Abs(move_v));
				rigidbody2D.velocity = new Vector2(move_h * maxSpeed, move_v * maxSpeed);
			}
			// handle above-ground movement
			else 
			{	
	            // horizontal control when the player is grounded or flying
	            //if (grounded || anim.GetBool("Fly"))
	            {
	            	move_h *= anim.GetBool("Grab") ? grabSpeed : (anim.GetBool("Mount") ? mountSpeed : 1.0f );
	                anim.SetFloat("Speed", Mathf.Abs(move_h));
	                anim.SetFloat("vSpeed", Mathf.Abs(rigidbody2D.velocity.y));
	                rigidbody2D.velocity = new Vector2(move_h == 0.0f ? rigidbody2D.velocity.x : move_h * maxSpeed, rigidbody2D.velocity.y);
				}					

	            // vertical control - jumping and flying
				if (jump && !dig)
				{
					// the player can jump while riding the reinder
					if (anim.GetBool("Mount"))
					{
						// add force until we leave the ground (note: grab and mount should be exclusive, check for sanity)
						if (anim.GetBool("Ground") && !anim.GetBool("Grab"))
							rigidbody2D.AddForce(new Vector2(0f, mountJumpForce));
					}
					// in some levels, the child can fly	
					else if ( canFly && !anim.GetBool("Grab"))
					{
						anim.SetBool("Fly", true);
						if (flaps > 0 && rigidbody2D.velocity.y < maxClimbSpeed && rigidbody2D.position.y < maxHeight )
							rigidbody2D.AddForce(new Vector2(0f, jumpForce*1.25f));
							flaps -= 1;
					}
				}
				else
				{
					anim.SetBool("Fly", false);
				}

			}

			// flip the character to face the direction of motion
	        if ( (move_h > 0 && !facingRight) || (move_h < 0 && facingRight) )
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