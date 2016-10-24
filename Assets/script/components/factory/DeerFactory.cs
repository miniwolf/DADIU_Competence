using Assets.script.animals;
using Assets.script.controllers;
using Assets.script.controllers.actions.damage;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.actions.notice;
using Assets.script.controllers.handlers;
using UnityEngine;

namespace Assets.script.components.factory {
	public class DeerFactory : Factory {
		private static Actionable deer;
		private static AnimalHandler handler;
		private static Camera camera;

		public DeerFactory(Actionable deer, AnimalHandler handler, Camera camera) {
			DeerFactory.deer = deer;
			DeerFactory.handler = handler;
			DeerFactory.camera = camera;
		}

		public void Build() {
			deer.AddAction(ControllableActions.Move, CreateMovement());
			deer.AddAction(ControllableActions.Roam, CreateRoaming());
			deer.AddAction(ControllableActions.Stop, CreateStop());
			deer.AddAction(ControllableActions.Flee, CreateFlee());
			deer.AddAction(ControllableActions.Damage, CreateDamage());
			deer.AddAction(ControllableActions.Notice, CreateNotice());
			deer.AddAction(ControllableActions.Nodanger, CreateNodanger());
		}

		private static Handler CreateNodanger() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new StopFleeing());
			return actionHandler;
		}

		private static Handler CreateNotice() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new FleeIfEnemy());
			return actionHandler;
		}

		private static Handler CreateDamage() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new TakeDamage(handler.GetAnimal()));
			actionHandler.AddAction(new DieIfNegativeLife(handler.GetAnimal()));
			actionHandler.AddAction(new FleeAwayFromAttacker(camera, handler));
			return actionHandler;
		}

		private static Handler CreateRoaming() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new RandomRoam(handler));
			return actionHandler;
		}

		private static Handler CreateFlee() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new FleeAwayFromAttacker(camera, handler));
			actionHandler.AddAction(new AnimalMovement(handler));
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
