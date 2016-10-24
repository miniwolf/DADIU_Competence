using System.Collections;
using AssemblyCSharp;
using Assets.script.components;
using Assets.script.components.registers;
using Assets.script.controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.script.animals {
	public class Deer : MonoBehaviour, GameEntity, AnimalHandler {
		private static readonly Animal animal = new AnimalImpl();
		private static readonly Actionable actionableAnimal = (Actionable) animal;
		private bool started;
		public float speed = 4;
		public bool Danger { get; set; }

		protected void Awake() {
			InjectionRegister.Register(this);
		}

		protected void OnDestroy() {
			animal.Destroy();
		}

		protected void Update() {
			actionableAnimal.ExecuteAction(Danger ? ControllableActions.Flee : ControllableActions.Move);

			if ( started ) {
				return;
			}

			StartCoroutine(DoAction());
			started = true;
		}

		private static IEnumerator DoAction() {
			while ( true ) {
				switch ( Random.Range(1, 4) ) {
					case 1:
					case 2:
						actionableAnimal.ExecuteAction(ControllableActions.Roam);
						break;
					default:
						actionableAnimal.ExecuteAction(ControllableActions.Stop);
						break;
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

		public Animal GetAnimal() {
			return animal;
		}

		public Actionable GetActionable() {
			return actionableAnimal;
		}

		public float GetSpeed() {
			return speed;
		}
	}
}