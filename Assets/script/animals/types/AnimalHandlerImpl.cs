using Assets.script.components;
using UnityEngine;

namespace Assets.script.animals.types {
	public class AnimalHandlerImpl : MonoBehaviour, AnimalHandler{
		private readonly float speed;
		protected Animal animal;
		protected Actionable actionableAnimal;

		public Collider Target { get; set; }
		public Vector3 Direction { get; set; }

		public AnimalHandlerImpl(int life, float speed) {
			this.speed = speed;
			animal = new AnimalImpl(life);
			actionableAnimal = (Actionable) animal;
		}

		public Vector3 GetPosition() {
			return transform.position;
		}

		public Animal GetAnimal() {
			return animal;
		}

		public Actionable GetActionable() {
			return actionableAnimal;
		}

		public float GetSpeed()
		{
			return speed;
		}
	}
}