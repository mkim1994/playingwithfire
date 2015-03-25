using UnityEngine;
using System.Collections;

public class EndAnimation : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		//int layer = animator.GetLayerIndex("Base Layer");
		AnimatorStateInfo currentAnim = animator.GetCurrentAnimatorStateInfo(0);
		if ( currentAnim.IsName("End State") )
		{
			Debug.Log("Animation Over, go to next level");
		}
	}
}
