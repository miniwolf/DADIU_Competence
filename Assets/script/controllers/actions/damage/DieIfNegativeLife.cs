using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.damage {
	public class DieIfNegativeLife : Action {
		private readonly Animal animal;
		private GameObject go;
		private PlayerController player;

		public DieIfNegativeLife(Animal animal, PlayerController player) {
			this.animal = animal;
			this.player = player;
		}

		public void Setup(GameObject gameObject) {
			go = gameObject;
		}

		public void Execute() {
			if ( animal.Life <= 0 ) {
				go.SetActive(false);
				player.IncrementScore(1);
			}
		}
	}
}