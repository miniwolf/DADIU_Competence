using Assets.script.controllers.actions;
using UnityEngine;

namespace Assets.script.controllers.handlers {
	public interface Handler {
		/// <summary>
		/// Setups the components. Called from the script that has a list of handlers
		/// </summary>
		/// <param name="obj">Gameobject that will execute the handler</param>
		void SetupComponents(GameObject obj);

		/// <summary>
		/// Adds an action to the handler. This action will be execute in order by the internal handler
		/// </summary>
		/// <param name="action">Action to be added.</param>
		void AddAction(Action action);

		/// <summary>
		/// Execute the actions in the container in order they have been added
		/// </summary>
		void DoAction();
	}
}
