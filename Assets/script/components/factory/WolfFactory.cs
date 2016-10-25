using Assets.script.animals;
using Assets.script.controllers;
using Assets.script.controllers.actions.damage;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.actions.notice;
using Assets.script.controllers.handlers;

namespace Assets.script.components.factory {
	public class WolfFactory {
		private static Actionable wolf;
		private static AnimalHandler handler;

		public WolfFactory(Actionable wolf, AnimalHandler handler) {
			WolfFactory.wolf = wolf;
			WolfFactory.handler = handler;
		}

		public void Build() {
			wolf.AddAction(ControllableActions.Move, CreateMovement());
			wolf.AddAction(ControllableActions.Roam, CreateRoaming());
			wolf.AddAction(ControllableActions.Stop, CreateStop());
			wolf.AddAction(ControllableActions.Damage, CreateDamage());
			wolf.AddAction(ControllableActions.Notice, CreateNotice());
			wolf.AddAction(ControllableActions.Nodanger, CreateNodanger());
			wolf.AddAction(ControllableActions.Hunt, CreateHunt());
		}

		private static Handler CreateHunt() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new HuntTarget(handler));
			actionHandler.AddAction(new AnimalMovement(handler));
			return actionHandler;
		}

		private static Handler CreateNodanger() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new StopHunting());
			return actionHandler;
		}

		private static Handler CreateNotice() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new HuntIfEnemy());
			return actionHandler;
		}

		private static Handler CreateDamage() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new TakeDamage(handler.GetAnimal()));
			actionHandler.AddAction(new DieIfNegativeLife(handler.GetAnimal()));
			actionHandler.AddAction(new HuntIfEnemy());
			return actionHandler;
		}

		private static Handler CreateRoaming() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new RandomRoam(handler));
			return actionHandler;
		}

		private static Handler CreateStop() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new StopMovement());
			return actionHandler;
		}

		private static Handler CreateMovement() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new AnimalMovement(handler));
			return actionHandler;
		}
	}
}