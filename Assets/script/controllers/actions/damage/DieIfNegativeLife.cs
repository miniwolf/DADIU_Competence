using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.damage {
	public class DieIfNegativeLife : Action {
		private readonly Animal animal;
		private GameObject go;

		public DieIfNegativeLife(Animal animal) {
			this.animal = animal;
		}

		public void Setup(GameObject gameObject) {
			go = gameObject;
		}

		public void Execute() {
			if ( animal.Life <= 0 ) {
				go.SetActive(false);
			}
		}
	}
}