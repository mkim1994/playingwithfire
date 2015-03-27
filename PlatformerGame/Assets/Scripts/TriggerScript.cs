using UnityEngine;

public class TriggerScript : MonoBehaviour {

	public void wake(){
		rigidbody2D.WakeUp ();
		}

	public void move(){
		Vector3 force = new Vector3 (100f, 0, 0);
		rigidbody2D.AddForce(force);
	}

	public void delete(){
		Destroy (gameObject);
	}


	public void drop(){
		transform.position = new Vector3 (32.9f, 3.2f, 0.0f);
	}

	public void rescale(){
		transform.localScale = new Vector3 (3.2f, 4.8f, 1.0f);
	}

	public void enable(){
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		renderer.enabled = true;
	}
	//add other functions here as neccessary 
	//put name of function as function variable in trigger stone to call it
	
}
