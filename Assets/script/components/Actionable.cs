using Assets.script.controllers;
using Assets.script.controllers.handlers;

namespace Assets.script.components {
	public interface Actionable {
		/// <summary>
		/// Adds the action along with the handler that will be executed when the command pattern invoker is called.
		/// </summary>
		/// <param name="actionName">Action name.</param>
		/// <param name="action">Action handler.</param>
		void AddAction(ControllableActions actionName, Handler action);

		/// <summary>
		/// Execute the specified handler bound to the actionanem.
		/// </summary>
		/// <param name="actionName">Action name.</param>
		void ExecuteAction(ControllableActions actionName);
	}
}
