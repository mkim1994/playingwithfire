using UnityEngine;
using System.Collections;

public class WallOfFire : MonoBehaviour {

	public float speed;
	private Vector3 start_position;
	private Collider2D collider;

	// Use this for initialization
	void Start () {
		start_position = transform.position;
		collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		transform.position = new Vector3( transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z );
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if ( other.tag == "Player" )
		{
			other.GetComponent<UnitySampleAssets._2D.Restarter>().DieInAFire();
			Reset();
		}
		else if ( other.tag == "Reindeer" )
		{
			Debug.Log( "YOUR REINDEER DIED IN A FIRE" );
			GameObject.FindWithTag("Player").GetComponent<UnitySampleAssets._2D.Restarter>().DieInAFire();
			Reset();
		}
		
	}

	void Reset() {
		transform.position = start_position;
	}
}
