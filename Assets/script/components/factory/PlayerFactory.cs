using Assets.script.controllers;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.handlers;
using UnityEngine;

namespace Assets.script.components.factory {
	public class PlayerFactory {
		private Camera camera;

		public PlayerFactory() {
			camera = GameObject.FindGameObjectWithTag(TagConstants.CAMERA).GetComponent<Camera>();
		}

		public void CreatePlayer(Actionable actionable) {
			actionable.AddAction(ControllableActions.Move, CreateMouseMovement());
		}

		private MouseMoveHandler CreateMouseMovement() {
			var move = new MouseMoveHandler(camera);
			move.AddMoveAction(new MoveActionImpl());
			return move;
		}

		public void CreateEnemy(Actionable actionable) {
		}
	}
}