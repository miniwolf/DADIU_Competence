using Assets.script.components;
using UnityEngine;

namespace Assets.script.animals {
	public interface AnimalHandler {
		Collider Target { get; set; }
		Vector3 Direction { get; set; }
		Vector3 GetPosition();
		Animal GetAnimal();
		Actionable GetActionable();
		float GetSpeed();
	}
}
