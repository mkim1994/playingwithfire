using UnityEngine;

namespace UnitySampleAssets._2D
{
	public class Raindeer : MonoBehaviour {

		public Transform child;
		public PlatformerCharacter2D childscript;
		private bool mounted = false;
		
		void Update () {
			if (Input.GetKey (KeyCode.Z) && !(mounted)) { //mount
				if (Vector3.Distance (child.position, transform.renderer.bounds.center) < 1f) {
					childscript.Mount(transform.position); //mount the raindeer
					GetComponent<SpriteRenderer>().renderer.enabled = false; //hide the raindeer
					mounted = true;
				}
			}
			if (Input.GetKey(KeyCode.X) && (mounted) && (child.rigidbody2D.velocity.y == 0)){ //dismount
				GetComponent<SpriteRenderer>().renderer.enabled = true; //hide the raindeer
				transform.position = child.position; //appear at mounted childs position
				childscript.Dismount();
				mounted = false;
			}
		}
	}
}