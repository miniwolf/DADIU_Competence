using Assets.script.components;
using UnityEngine;

namespace Assets.script.animals.types {
	public class Wolf : MonoBehaviour, GameEntity, AnimalHandler {
		private Animal animal = new AnimalImpl(2);

		public string GetTag() {
			return TagConstants.WOLF;
		}

		public void SetupComponents() {
			animal.SetupHandlers(gameObject);
		}

		public Animal GetAnimal() {
			throw new System.NotImplementedException();
		}

		public Actionable GetActionable() {
			throw new System.NotImplementedException();
		}

		public float GetSpeed() {
			throw new System.NotImplementedException();
		}
	}
}