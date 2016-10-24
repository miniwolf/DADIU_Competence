using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class AnimalMovement : Action {
		private CharacterController characterController;
		private readonly AnimalHandler handler;

		private const float GRAVITY = 9.8f;

		public AnimalMovement(AnimalHandler handler) {
			this.handler = handler;
		}

		public void Setup(GameObject gameObject) {
			characterController = gameObject.GetComponent<CharacterController>();
		}

		public void Execute() {
			if ( characterController.isGrounded ) {
				characterController.Move(handler.Direction * Time.deltaTime);
			} else {
				characterController.Move(new Vector3(0, -GRAVITY, 0) * Time.deltaTime);
			}
		}
	}
}