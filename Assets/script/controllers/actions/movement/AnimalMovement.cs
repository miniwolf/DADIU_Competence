using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class AnimalMovement : Action {
		private CharacterController characterController;
		private AnimalHandler animal;
		private const float GRAVITY = 9.8f;

		public void Setup(GameObject gameObject) {
			characterController = gameObject.GetComponent<CharacterController>();
			animal = gameObject.GetComponent<AnimalHandler>();
		}

		public void Execute() {
			if ( characterController.isGrounded ) {
				characterController.Move(animal.GetAnimal().Direction * Time.deltaTime);
			} else {
				characterController.Move(new Vector3(0, -GRAVITY, 0) * Time.deltaTime);
			}
		}
	}
}