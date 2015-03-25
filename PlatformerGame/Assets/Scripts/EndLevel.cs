using UnityEngine;

public class EndLevel : MonoBehaviour {

	public Fader fade;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			fade.EndScene();
			gameObject.collider2D.enabled = false;
		}
	}
}
