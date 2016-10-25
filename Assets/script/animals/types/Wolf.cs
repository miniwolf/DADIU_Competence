using System.Collections;
using Assets.script.components.registers;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.script.animals.types {
	public class Wolf : AnimalHandlerImpl, GameEntity {
		private bool started;
		public bool Noticed { get; set; }
		public bool CanKill { get; set; }
		public bool CanMove { get; set; }

		public Wolf() : base(2, 9) { // health, speed
		}

		protected void Awake() {
			InjectionRegister.Register(this);
			CanMove = true;
			CanKill = true;
		}

		protected void OnDestroy() {
			animal.Destroy();
		}

		protected void Update() {
			if ( !CanMove ) {
				return;
			}
			actionableAnimal.ExecuteAction(Noticed ? ControllableActions.Hunt : ControllableActions.Move);

			if ( started ) {
				return;
			}

			StartCoroutine(DoAction(this));
			started = true;
		}

		private static IEnumerator DoAction(Wolf wolf) {
			while ( true ) {
				if ( !wolf.Noticed ) {
					wolf.actionableAnimal.ExecuteAction(ControllableActions.Roam);
				}
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