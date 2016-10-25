using Assets.script.animals.types;
using UnityEngine;

namespace Assets.script.controllers.actions.notice {
	public class FleeIfEnemy : Action {
		private Deer deer;

		public void Setup(GameObject gameObject) {
			deer = gameObject.GetComponent<Deer>();
		}

		public void Execute() {
			switch ( deer.Target.tag ) {
				case TagConstants.PLAYER:
				case TagConstants.WOLF:
					deer.Danger = true;
					break;
				default:
					deer.Target = null;
					break;
			}

		}
	}
}