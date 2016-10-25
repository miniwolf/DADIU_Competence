using System.Collections;
using Assets.script.components.registers;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.script.animals.types {
	public class Wolf : AnimalHandlerImpl, GameEntity {
		private bool started;

		public Wolf() : base(4,2) {
		}

		protected void Awake() {
			InjectionRegister.Register(this);
		}

		protected void OnDestroy() {
			animal.Destroy();
		}

		protected void Update() {
			actionableAnimal.ExecuteAction(ControllableActions.Move);

			if ( started ) {
				return;
			}

			StartCoroutine(DoAction());
			started = true;
		}

		private static IEnumerator DoAction() {
			while ( true ) {
				actionableAnimal.ExecuteAction(ControllableActions.Roam);
				yield return new WaitForSeconds(2);
			}
		}

		public string GetTag() {
			return TagConstants.WOLF;
		}

		public void SetupComponents() {
			animal.SetupHandlers(gameObject);
		}
	}
}