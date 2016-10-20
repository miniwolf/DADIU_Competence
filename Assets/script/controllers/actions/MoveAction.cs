using UnityEngine;

namespace AssemblyCSharp {
	public interface MoveAction : Action {
		/// <summary>
		/// Will pass the position on to the action to affect something on the given position.
		/// </summary>
		/// <param name="pos">Position clicked.</param>
		void Execute(Vector3 pos);
	}
}
