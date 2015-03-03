using UnityEngine;

namespace UnitySampleAssets._2D
{
    public class Restarter : MonoBehaviour
    {
		public PlatformerCharacter2D character;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Respawn"){
                //Application.LoadLevel(Application.loadedLevelName);
				Invoke ("Respawn", 0.6f);} //delay before respawn
			if (other.tag == "SpawnStone") {
				character.spawnPoint = other.transform.position;
				}
        }
		private void Respawn(){
			character.Respawn ();
		}
    }
}