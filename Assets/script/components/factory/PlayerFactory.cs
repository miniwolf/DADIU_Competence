using UnityEngine;

namespace AssemblyCSharp {
	public class PlayerFactory {
		private GameObject playerObj;
		private Camera camera;

		public PlayerFactory() {
			camera = GameObject.FindGameObjectWithTag(TagConstants.CAMERA).GetComponent<Camera>();
			playerObj = GameObject.FindGameObjectWithTag(TagConstants.PLAYER);
		}

		public void CreatePlayer(Actionable actionable) {
			actionable.AddAction(ControllableActions.MOVE, CreateMouseMovement(actionable));
		}

		private MouseMoveHandler CreateMouseMovement(Actionable actionable) {
			MouseMoveHandler move = new MouseMoveHandler(camera);
			move.AddMoveAction(new MoveActionImpl());
			return move;
		}

		public void CreateEnemy(Actionable actionable) {
		}
	}
}