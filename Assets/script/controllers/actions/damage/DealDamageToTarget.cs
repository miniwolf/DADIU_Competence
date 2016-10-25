using Assets.script.animals.types;
using Assets.script.components;
using UnityEngine;

namespace Assets.script.controllers.actions.damage {
	public class DealDamageToTarget : Action {
		private readonly Wolf wolf;

		public DealDamageToTarget(Wolf wolf) {
			this.wolf = wolf;
		}

		public void Setup(GameObject gameObject) {
		}

		public void Execute() {
			wolf.Target.gameObject.GetComponent<Damageable>().TakeDamage(20);
		}
	}
}