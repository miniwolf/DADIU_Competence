using Assets.script;
using Assets.script.animals;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.Longbow.Scripts.CS {
	public class ArrowStuck : MonoBehaviour {
		private RaycastHit hit;
		public AudioClip hitSound;
		//sound to play when arrow hits a surface

		// Update is called once per frame
		protected void Update() {
			//Only check for obstacles to stick to if the arrow is moving
			if ( GetComponent<Rigidbody>().velocity.magnitude <= 0.5 ) {
				enabled = false;
			}
		}

		private void CheckForObstacles(Vector3 pos, Transform trans) {
			var audioSource = GetComponent<AudioSource>();
			audioSource.Stop();
			audioSource.clip = hitSound;
			audioSource.Play();

			//disable arrow's collider | if you leave the arrow's collider on, while inside another object, it may have strange effects
			GetComponent<BoxCollider>().enabled = false;

			//freeze rigidbody
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			//position arrow
			transform.position = pos;
			//parent arrow to object | if the object the arrow hits moves, this will make the arrow move with it
			transform.parent = trans;

			//once the arrow is stuck, disable this script
			enabled = false;
		}

		protected void OnCollisionEnter(Collision collision) {
			var velocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;
			if ( velocityMagnitude <= 2 || collision.transform.tag == TagConstants.PLAYER) {
				return;
			}

			Debug.Log("Arrow hit: " + collision.transform.tag + ", collision.gameObject.tag: " + collision.gameObject.tag);
			var handler = collision.gameObject.GetComponentInParent<AnimalHandler>();
			if ( handler != null ) {
				handler.GetActionable().ExecuteAction(ControllableActions.Damage);
			}

			var pos = Vector3.zero;
			foreach ( var contact in collision.contacts ) {
				pos = contact.point;
			}
			CheckForObstacles(pos, collision.transform);
		}
	}
}
