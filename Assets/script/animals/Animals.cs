using UnityEngine;

namespace Assets.script.animals {
	public interface Animal {
		Vector3 Direction { get; set; }
		void Destroy();
		void SetupHandlers(GameObject go);
		void TakeDamage(int damage);
		int Life { get; }
	}
}
