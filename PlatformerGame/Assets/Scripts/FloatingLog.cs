using UnityEngine;
using System.Collections;

public class FloatingLog : MonoBehaviour {

	public float bouyancy = 70f;
	public float floatSpeed = 1.0f;
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Water") 
		{	
			if (transform.position.y - other.transform.position.y < 2.1f){
			rigidbody2D.AddForce(Vector3.up * bouyancy);
			rigidbody2D.velocity = new Vector2(floatSpeed*rigidbody2D.velocity.x/Mathf.Abs(rigidbody2D.velocity.x), rigidbody2D.velocity.y*0.99f);
				if (Mathf.Abs(rigidbody2D.velocity.x) <= 0.1f) {
					rigidbody2D.velocity = new Vector2(Random.Range(-1f,1f),1f);
				}
			}
		}
	}
}
