using UnityEngine;

namespace UnitySampleAssets._2D
{

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;
		private Vector3 startPos;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
			startPos = character.transform.position;
        }

        private void Update()
        {
            if (!jump)
            // Read the jump input in Update so button presses aren't missed.
			jump = Input.GetKey (KeyCode.UpArrow);

        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.DownArrow);
            float h = 0;
			if (Input.GetKey(KeyCode.LeftArrow)){ h = -0.35f;}
			if (Input.GetKey(KeyCode.RightArrow)){ h = 0.35f;}
			if (Input.GetKey(KeyCode.UpArrow)) {character.Climb();}
            // Pass all parameters to the character control script.
            character.Move(h, crouch, jump);
            jump = false;
			if (Input.GetKey(KeyCode.R)){character.transform.position = startPos;}
        }
		private	void OnCollisionStay2D(Collision2D coll) {
			if (coll.gameObject.tag == "Breakable"){
				if (Input.GetKey(KeyCode.LeftControl)){character.Bash(coll.gameObject);}
			}
    	}
	}
}