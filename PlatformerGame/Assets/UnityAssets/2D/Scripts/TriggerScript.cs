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

	//add other functions here as neccessary 
	//put name of function as function variable in trigger stone to call it
	
}
