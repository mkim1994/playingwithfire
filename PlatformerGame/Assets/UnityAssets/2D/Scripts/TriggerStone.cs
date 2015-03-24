using UnityEngine;

public class TriggerStone : MonoBehaviour {

	public Sprite ActiveStone;
	public SpriteRenderer renderer;
	public Transform child;
	public TriggerScript script;
	public string function;

	void Update () {
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (Vector3.Distance (child.position, transform.renderer.bounds.center) < 1f) {
				renderer.sprite = ActiveStone;
				collider2D.enabled = false; //dissable from being set again
				if(script != null)
					script.Invoke(function,0f);
			}
		}
	}
}
