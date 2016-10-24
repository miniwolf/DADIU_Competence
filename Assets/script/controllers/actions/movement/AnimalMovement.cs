using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class AnimalMovement : Action {
		private CharacterController characterController;
		private AnimalHandler handler;

		private const float GRAVITY = 9.8f;

		public void Setup(GameObject gameObject) {
			handler = gameObject.GetComponent<AnimalHandler>();
			characterController = gameObject.GetComponent<CharacterController>();
		}

		public void Execute() {
			if ( characterController.isGrounded ) {
				characterController.Move(handler.GetAnimal().Direction * Time.deltaTime);
			} else {
				characterController.Move(new Vector3(0, -GRAVITY, 0) * Time.deltaTime);
			}
		}
	}
}