using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class StopMovement : Action {
		private CharacterController characterController;

		public void Setup(GameObject gameObject) {
			characterController = gameObject.GetComponent<CharacterController>();
		}

		public void Execute() {
			characterController.Move(Vector3.zero);
		}
	}
}