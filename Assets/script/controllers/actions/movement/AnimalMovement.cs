using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class AnimalMovement : Action {
		private CharacterController characterController;
		private readonly AnimalHandler handler;
		private GameObject go;

		private const float GRAVITY = 9.8f;

		public AnimalMovement(AnimalHandler handler) {
			this.handler = handler;
		}

		public void Setup(GameObject gameObject) {
			characterController = gameObject.GetComponent<CharacterController>();
			go = gameObject;
		}

		public void Execute() {
			if ( characterController.isGrounded ) {
				characterController.Move(handler.Direction * Time.deltaTime);
			} else {
				if ( go.transform.position.y < 0 ) {
					go.transform.position = new Vector3(go.transform.position.x, 10, go.transform.position.z);
				}
				characterController.Move(new Vector3(0, -GRAVITY, 0) * Time.deltaTime);
			}
		}
	}
}