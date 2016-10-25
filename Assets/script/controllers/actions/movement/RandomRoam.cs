using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class RandomRoam : Action {
		private AnimalHandler handler;
		private const float WATER_HEIGHT = .5f;

		public RandomRoam(AnimalHandler handler) {
			this.handler = handler;
		}

		public void Setup(GameObject gameObject) {
			handler = gameObject.GetComponent<AnimalHandler>();
		}

		public void Execute() {
			if ( handler.GetPosition().y < WATER_HEIGHT ) {
				handler.Direction = new Vector3(-handler.Direction.x, 0, -handler.Direction.z);
			} else switch (Random.Range(1, 6)) {
				case 1:
					handler.Direction = new Vector3(handler.GetSpeed(), 0, 0);
					break;
				case 2:
					handler.Direction = new Vector3(-handler.GetSpeed(), 0, 0);
					break;
				case 3:
					handler.Direction = new Vector3(0, 0, handler.GetSpeed());
					break;
				case 4:
					handler.Direction = new Vector3(0, 0, -handler.GetSpeed());
					break;
				case 5:
					handler.Direction = new Vector3(0, 0, 0);
					break;
			}
		}
	}
}