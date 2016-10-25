using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.damage {
	public class FleeAwayFromAttacker : Action {
		private readonly AnimalHandler handler;

		public FleeAwayFromAttacker(Camera camera, AnimalHandler handler) {
			this.handler = handler;
		}

		public void Setup(GameObject gameObject) {
		}

		public void Execute() {
			var enemyLocation = handler.Target.transform.position;
			var ourLocation = handler.GetPosition();
			var inverseDirection = ourLocation - enemyLocation;
			inverseDirection.y = 0;
			handler.Direction = inverseDirection.normalized * handler.GetSpeed();
//			Debug.Log(handler.Direction);
		}
	}
}