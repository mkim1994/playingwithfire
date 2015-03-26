using UnityEngine;
using System.Collections;

public class FloatingLog : MonoBehaviour {

	public float bouyancy = 70f;
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Water") 
		{	
			if (transform.position.y - other.transform.position.y < 2.1f){
			rigidbody2D.AddForce(Vector3.up * bouyancy);
			rigidbody2D.velocity = new Vector2(0.75f*rigidbody2D.velocity.x/Mathf.Abs(rigidbody2D.velocity.x), rigidbody2D.velocity.y*0.99f);

			}
		}
	}
}
