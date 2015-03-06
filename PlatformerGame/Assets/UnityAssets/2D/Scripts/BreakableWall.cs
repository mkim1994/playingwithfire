using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour {

	public float minSpeed = 3.0f;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && Mathf.Abs(other.rigidbody2D.velocity.x) > minSpeed) { //moving fast enough (on raindeer)
			other.rigidbody2D.velocity.Set(0,other.rigidbody2D.velocity.y); // slow down raindeer
			Destroy(gameObject); //destroy self
		}	
		else{other.rigidbody2D.velocity.Set(0,other.rigidbody2D.velocity.y);} //doesnt work for some reason...?
	}
}
