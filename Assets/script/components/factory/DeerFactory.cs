using AssemblyCSharp;
using Assets.script.animals;
using Assets.script.controllers;
using Assets.script.controllers.actions.damage;
using Assets.script.controllers.actions.movement;
using Assets.script.controllers.handlers;
using UnityEngine;

namespace Assets.script.components.factory {
	public class DeerFactory : Factory {
		private readonly Actionable deer;
		private static Animal animal;

		public DeerFactory(AnimalHandler deer) {
			animal = deer.GetAnimal();
			this.deer = deer.GetActionable();
		}

		public void Build() {
			deer.AddAction(ControllableActions.Move, CreateMovement());
			deer.AddAction(ControllableActions.Roam, CreateRoaming());
			deer.AddAction(ControllableActions.Stop, CreateStop());
			deer.AddAction(ControllableActions.Flee, CreateFlee());
			deer.AddAction(ControllableActions.Damage, CreateDamage());
		}

		private static Handler CreateDamage() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new TakeDamage(animal));
			actionHandler.AddAction(new DieIfNegativeLife(animal));
			var camera = GameObject.FindGameObjectWithTag(TagConstants.CAMERA).GetComponent<Camera>();
			actionHandler.AddAction(new FleeAwayFromAttacker(camera));
			return actionHandler;
		}

		private static Handler CreateRoaming() {
			var actionHandler = new ActionHandler();
			actionHandler.AddAction(new RandomRoam(animal));
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
