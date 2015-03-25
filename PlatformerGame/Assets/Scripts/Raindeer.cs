﻿using UnityEngine;

namespace UnitySampleAssets._2D
{
	public class Raindeer : MonoBehaviour {

		public Transform child;
		public PlatformerCharacter2D childscript;
		private bool mounted = false;
		private Vector3 startPos;
		private Vector2 zeroVector;

		private void Awake()
		{
			startPos = transform.position;
			zeroVector = new Vector2 (0f, 0f);
		}
		
		void Update () {
			if (Input.GetKey(KeyCode.R)){transform.position = startPos;}
			if (mounted){transform.position = child.position + new Vector3(0,0.7f,0);}
			if (Input.GetKeyDown (KeyCode.LeftShift) && child.rigidbody2D.velocity == zeroVector) {
				if(!mounted){ //mount
					if (Vector3.Distance (child.position, transform.renderer.bounds.center) < 1f) {
						childscript.Mount(transform.position); //mount the raindeer
						GetComponent<SpriteRenderer>().renderer.enabled = false; //hide the raindeer
						mounted = true;

					}
				}
				else if((mounted) && (child.rigidbody2D.velocity.y == 0)){ //dismount
					GetComponent<SpriteRenderer>().renderer.enabled = true; //hide the raindeer
					transform.position = child.position + new Vector3(0,0.7f,0); //appear at mounted childs position
					//transform.position+= new Vector3(0,10,0);
					childscript.Dismount();
					mounted = false;
				}
			}
		}
	}
}