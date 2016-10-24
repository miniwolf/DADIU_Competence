using AssemblyCSharp;
using Assets.script.controllers;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.handlers;

namespace Assets.script.components.factory {
	public class DeerFactory : Factory {
		private readonly Actionable deer;

		public DeerFactory(Actionable deer) {
			this.deer = deer;
		}

		public void Build() {
			deer.AddAction(ControllableActions.Move, CreateMovement());
			deer.AddAction(ControllableActions.Roam, CreateRoaming());
			deer.AddAction(ControllableActions.Stop, CreateStop());
			deer.AddAction(ControllableActions.Flee, CreateFlee());
		}

		private static Handler CreateRoaming() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new RandomRoam());
			return actionHandler;
		}

		private static Handler CreateFlee() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new AnimalMovement());
			return actionHandler;
		}

		private static Handler CreateStop() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new StopMovement());
			return actionHandler;
		}

		private static Handler CreateMovement() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new AnimalMovement());
			return actionHandler;
		}
	}
}
