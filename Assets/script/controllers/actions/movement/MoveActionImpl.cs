using UnityEngine;

namespace AssemblyCSharp {
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
			this.agent = objAgent;
		}

		public void Execute() {
		}
	}
}
