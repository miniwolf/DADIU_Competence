using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.damage {
	public class TakeDamage : Action {
		private readonly Animal animal;
		private readonly int damage;

		public TakeDamage(Animal animal) {
			this.animal = animal;
			damage = 1;
		}

		public void Setup(GameObject gameObject) {
		}

		public void Execute() {
			animal.TakeDamage(damage);
		}
	}
}