using System;

namespace AssemblyCSharp {
	using UnityEngine;
	using System.Collections.Generic;

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
			foreach ( MoveAction action in moveActions ) {
				action.Setup(obj);
			}
			foreach ( Action action in actions ) {
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
			if ( Input.GetMouseButtonDown(0) ) {	
				cameraToGround = camera.ScreenPointToRay(Input.mousePosition);
				if ( Physics.Raycast(cameraToGround, out hit, 500f, layerMask.value) ) {
					foreach ( MoveAction action in moveActions ) {
						action.Execute(hit.point);
					}
					foreach ( Action action in actions ) {
						action.Execute();
					}
				}
			}
		}
	}
}

