using Assets.script.components;

namespace Assets.script.animals {
	public interface AnimalHandler {
		Animal GetAnimal();
		Actionable GetActionable();
		float GetSpeed();
	}
}
