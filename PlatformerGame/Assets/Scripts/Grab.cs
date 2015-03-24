using UnityEngine;
using System.Collections;

public class Grab : MonoBehaviour {

	public bool grabbed = false;
	public Animator anim;
	public Transform child;

	// Update is called once per frame
	void Update () {
		HingeJoint2D hinge = gameObject.GetComponent<HingeJoint2D>();
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (Vector3.Distance (child.position, transform.renderer.bounds.center) < 1.5f && anim.GetBool("Crouch") == false) {
					grabbed = true;
					hinge.enabled = true;
			} //bind the object to the player
		}
		else { 
			grabbed = false;
			hinge.enabled = false;} //let go
		anim.SetBool("Grab", grabbed);
		}
	}