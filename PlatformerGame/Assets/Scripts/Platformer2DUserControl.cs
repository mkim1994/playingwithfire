using UnityEngine;

namespace UnitySampleAssets._2D
{

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;
		private Vector3 startPos;
		//private bool rabbitAspect;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
			startPos = character.transform.position;
        }

        private void Update()
        {
            // Read the jump input in Update so button presses aren't missed.
            if (!jump)
           	    jump = Input.GetKeyDown(KeyCode.UpArrow);

        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool dig = Input.GetKey(KeyCode.DownArrow);
            float h = 0;
            float v = 0;
			if (Input.GetKey(KeyCode.LeftArrow)){ h = -0.35f;}
			else if (Input.GetKey(KeyCode.RightArrow)){ h = 0.35f;}
			if (Input.GetKey(KeyCode.UpArrow)) { v = 0.35f; character.Climb();}
            else if (Input.GetKey(KeyCode.DownArrow)){ v = -0.35f;}
            // Pass all parameters to the character control script.
            character.Move(h, v, dig, jump);
            jump = false;
			if (Input.GetKey(KeyCode.R)){character.Respawn();}
        }
		private	void OnCollisionStay2D(Collision2D coll) {
			if (coll.gameObject.tag == "Breakable"){
				if (Input.GetKey(KeyCode.LeftControl)){character.Bash(coll.gameObject);}
			}

    	}
	}
}