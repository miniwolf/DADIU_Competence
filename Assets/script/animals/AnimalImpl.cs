using System.Collections.Generic;
using Assets.script.components;
using Assets.script.controllers;
using Assets.script.controllers.handlers;
using UnityEngine;

namespace Assets.script.animals {
	public class AnimalImpl : Animal, Actionable  {
		private readonly Dictionary<ControllableActions, Handler> actions =
			new Dictionary<ControllableActions, Handler>();

		public Vector3 Direction { get; set; }

		public void Destroy() {
			actions.Clear();
		}

		public void SetupHandlers(GameObject go) {
			foreach (var actionsValue in actions.Values) {
				actionsValue.SetupComponents(go);
			}
		}

		public void AddAction(ControllableActions actionName, Handler action) {
			actions.Add(actionName, action);
		}

		public void ExecuteAction(ControllableActions actionName) {
			actions[actionName].DoAction();
		}
	}
}
