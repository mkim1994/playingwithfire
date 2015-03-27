using UnityEngine;

public class DigTrigger : MonoBehaviour {
	
	public Transform child;
	public TriggerScript tiles;
	public TriggerScript sprites;
	public string tileFunction;
	public string spriteFunction;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			gameObject.collider2D.enabled = false;
			if(tiles != null)
				tiles.Invoke (tileFunction, 0f);
			if(sprites != null)
				sprites.Invoke (spriteFunction, 0f);
		}
	}
}
