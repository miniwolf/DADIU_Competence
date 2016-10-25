using Assets.script.animals.types;
using UnityEngine;

namespace Assets.script.controllers.actions.notice {
	public class StopFleeing : Action {
		private Deer deer;

		public void Setup(GameObject gameObject) {
			deer = gameObject.GetComponent<Deer>();
		}

		public void Execute() {
			deer.Danger = false;
			deer.Target = null;
		}
	}
}