using System.Collections;
using Assets.script.components.registers;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.script.animals.types {
	public class Deer : AnimalHandlerImpl, GameEntity {
		public bool Danger { get; set; }
		private bool started;
		private bool initialized;

		public Deer() : base(1, 4.5f) { // health, speed
		}

		protected void Awake() {
			initialized = true;
			InjectionRegister.Register(this);
		}

		protected void OnDestroy() {
			animal.Destroy();
		}

		protected void Update() {
			if ( !initialized ) {
				Debug.Log("Issue");
			}
			actionableAnimal.ExecuteAction(Danger ? ControllableActions.Flee : ControllableActions.Move);

			if ( started ) {
				return;
			}

			StartCoroutine(DoAction(this));
			started = true;
		}

		private static IEnumerator DoAction(Deer deer) {
			while ( true ) {
				if ( !deer.Danger ) {
					deer.actionableAnimal.ExecuteAction(ControllableActions.Roam);
				}
				yield return new WaitForSeconds(2);
			}
		}

		public void SetupComponents() {
			animal.SetupHandlers(gameObject);
		}

		public string GetTag() {
			return TagConstants.DEER;
		}
	}
}