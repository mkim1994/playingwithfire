using UnityEngine;

public class SpawnStone : MonoBehaviour {

	public Sprite ActiveStone;
	public SpriteRenderer renderer;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			renderer.sprite = ActiveStone;
			collider2D.enabled = false; //dissable from being set again
		}
	}
}
