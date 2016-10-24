using System.Collections.Generic;
using Assets.script.controllers.actions;
using UnityEngine;
using Action = Assets.script.controllers.actions.Action;

namespace Assets.script.controllers.handlers {
	public class MouseMoveHandler : Handler {
		private LayerMask layerMask;// = 1 << LayerConstants.GroundLayer;
		private RaycastHit hit;
		private Ray cameraToGround;

		private readonly Camera camera;
		private readonly List<MoveAction> moveActions = new List<MoveAction>();
		private readonly List<Action> actions = new List<Action>();

		public MouseMoveHandler(Camera camera) {
			this.camera = camera;
		}

		public void SetupComponents(GameObject obj) {
			foreach ( var action in moveActions ) {
				action.Setup(obj);
			}
			foreach ( var action in actions ) {
				action.Setup(obj);
			}
		}

		public void AddAction(Action action) {
			actions.Add(action);
		}

		public void AddMoveAction(MoveAction action) {
			moveActions.Add(action);
		}

		public void DoAction() {
			if ( !Input.GetMouseButtonDown(0) ) {
				return;
			}

			cameraToGround = camera.ScreenPointToRay(Input.mousePosition);
			if ( !Physics.Raycast(cameraToGround, out hit, 500f, layerMask.value) ) {
				return;
			}

			foreach ( var action in moveActions ) {
				action.Execute(hit.point);
			}
			foreach ( var action in actions ) {
				action.Execute();
			}
		}
	}
}

