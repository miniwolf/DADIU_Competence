﻿using Assets.script.animals;
using UnityEngine;

namespace Assets.script.controllers.actions.movement {
	public class RandomRoam : Action {
		private Animal animal;
		private AnimalHandler animalHandler;

		public void Setup(GameObject gameObject) {
			animalHandler = gameObject.GetComponent<AnimalHandler>();
			animal = animalHandler.GetAnimal();
		}

		public void Execute() {
			switch (Random.Range(1, 4)) {
				case 1:
					animal.Direction = new Vector3(animalHandler.GetSpeed(), 0, 0);
					break;
				case 2:
					animal.Direction = new Vector3(-animalHandler.GetSpeed(), 0, 0);
					break;
				case 3:
					animal.Direction = new Vector3(0, 0, animalHandler.GetSpeed());
					break;
				case 4:
					animal.Direction = new Vector3(0, 0, -animalHandler.GetSpeed());
					break;
			}
		}
	}
}