using Assets.script.animals;
using Assets.script.animals.types;
using Assets.script.controllers;
using Assets.script.controllers.actions.damage;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.actions.notice;
using Assets.script.controllers.handlers;
using Assets.script.player;

namespace Assets.script.components.factory {
	public class WolfFactory {
		private static Actionable actionable;
		private static AnimalHandler handler;
		private static PlayerController player;
		private static Wolf wolf;

		public WolfFactory(Actionable actionable, AnimalHandler handler, PlayerController player, Wolf wolf) {
			WolfFactory.wolf = wolf;
			WolfFactory.actionable = actionable;
			WolfFactory.handler = handler;
			WolfFactory.player = player;
		}

		public void Build() {
			actionable.AddAction(ControllableActions.Move, CreateMovement());
			actionable.AddAction(ControllableActions.Roam, CreateRoaming());
			actionable.AddAction(ControllableActions.Stop, CreateStop());
			actionable.AddAction(ControllableActions.Damage, CreateDamage());
			actionable.AddAction(ControllableActions.Notice, CreateNotice());
			actionable.AddAction(ControllableActions.Nodanger, CreateNodanger());
			actionable.AddAction(ControllableActions.Hunt, CreateHunt());
			actionable.AddAction(ControllableActions.Kill, CreateKill());
		}

		private static Handler CreateKill() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new DealDamageToTarget(wolf));
			actionHandler.AddAction(new DisableMovement(wolf));
			return actionHandler;
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
			actionHandler.AddAction(new DieIfNegativeLife(handler.GetAnimal(), player));
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