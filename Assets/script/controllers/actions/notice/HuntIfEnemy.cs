using Assets.script.animals.types;
using UnityEngine;

namespace Assets.script.controllers.actions.notice {
	public class HuntIfEnemy : Action {
		private Wolf wolf;

		public void Setup(GameObject gameObject) {
			wolf = gameObject.GetComponent<Wolf>();
		}

		public void Execute() {
			switch ( wolf.Target.tag ) {
				case TagConstants.PLAYER:
				case TagConstants.DEER:
					wolf.Noticed = true;
					break;
				default:
					wolf.Target = null;
					break;
			}

		}
	}
}