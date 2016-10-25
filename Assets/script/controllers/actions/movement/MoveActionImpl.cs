
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class MoveActionImpl : MoveAction {
		private NavMeshAgent agent;

		public void Execute(Vector3 pos) {
			agent.destination = pos;
			agent.Resume();
		}

		public void Setup(GameObject gameObject) {
			var objAgent = gameObject.GetComponent<NavMeshAgent>();
			if ( objAgent ?? false ) {
				Debug.LogError(gameObject.name + " should have a navmeshagent");
				return;
			}
			agent = objAgent;
		}

		public void Execute() {
		}
	}
}
