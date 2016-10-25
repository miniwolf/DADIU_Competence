using Assets.script.animals.types;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class DisableMovement : Action {
		private readonly Wolf wolf;

		public DisableMovement(Wolf wolf) {
			this.wolf = wolf;
		}

		public void Setup(GameObject gameObject) {
		}

		public void Execute() {
			wolf.CanMove = false;
		}
	}
}