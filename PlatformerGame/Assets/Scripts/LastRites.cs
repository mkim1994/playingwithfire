using UnityEngine;
using System.Collections;

public class LastRites : MonoBehaviour {

	public string state;
	private bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if ( !triggered )
		{
			other.GetComponent<UnitySampleAssets._2D.PlatformerCharacter2D>().Dismount();
			Animator animator = other.GetComponent<Animator>();
			animator.CrossFade( "Idle", 0.5f, 0 );
			animator.SetBool( state, true );
			animator.Play( "Kneel", 0, 1.0f );
			triggered = true;
		}
		
	}
}
