using UnityEngine;
using System.Collections;

public class Wake : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.tag == "Player"){
			this.rigidbody2D.fixedAngle = false;
		}
	}
}
