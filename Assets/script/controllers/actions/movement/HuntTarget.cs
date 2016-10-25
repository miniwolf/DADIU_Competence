using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class HuntTarget : Action {
		private readonly AnimalHandler handler;

		public HuntTarget(AnimalHandler handler) {
			this.handler = handler;
		}

		public void Setup(GameObject gameObject) {
		}

		public void Execute() {
			var enemyLocation = handler.Target.transform.position;
			var ourLocation = handler.GetPosition();
			var direction = enemyLocation - ourLocation;
			direction.y = 0;
			handler.Direction = direction.normalized * handler.GetSpeed();
		}
	}
}