using UnityEngine;

namespace Assets.script.animals {
	public interface Animal {
		void Destroy();
		void SetupHandlers(GameObject go);
		void TakeDamage(int damage);
		int Life { get; }
	}
}
