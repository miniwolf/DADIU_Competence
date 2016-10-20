using UnityEngine;

namespace AssemblyCSharp {
	/// <summary>
	/// The controller is responsible for response control
	/// </summary>
	public interface Action {
		/// <summary>
		/// Called from the Start in MonoBehaviour
		/// </summary>
		void Setup(GameObject gameObject);

		/// <summary>
		/// Command to be executed
		/// </summary>
		void Execute();
	}
}

