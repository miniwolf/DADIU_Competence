using Assets.script.animals.types;
using UnityEngine;

namespace Assets.script.controllers.actions.notice {
	public class StopHunting : Action {
		private Wolf wolf;

		public void Setup(GameObject gameObject) {
			wolf = gameObject.GetComponent<Wolf>();
		}

		public void Execute() {
			wolf.Noticed = false;
			wolf.Target = null;
		}
	}
}