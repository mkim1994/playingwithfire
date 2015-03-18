using UnityEngine;

namespace UnitySampleAssets._2D
{
	public class Raindeer : MonoBehaviour {

		public Transform child;
		public PlatformerCharacter2D childscript;
		private bool mounted = false;
		
		void Update () {
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				if(!mounted){ //mount
					if (Vector3.Distance (child.position, transform.renderer.bounds.center) < 1f) {
						childscript.Mount(transform.position); //mount the raindeer
						GetComponent<SpriteRenderer>().renderer.enabled = false; //hide the raindeer
						mounted = true;
					}
				}
				else if((mounted) && (child.rigidbody2D.velocity.y == 0)){
					GetComponent<SpriteRenderer>().renderer.enabled = true; //hide the raindeer
					transform.position = child.position; //appear at mounted childs position
					childscript.Dismount();
					mounted = false;
				}
			}
		}
	}
}