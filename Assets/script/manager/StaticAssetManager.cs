using UnityEngine;


/// <summary>
/// Interface that navigates static objects in the scene. May contain some caching mechanism (object pooling)
/// </summary>
public interface StaticAssetManager : MapChangeListener {
	/// <summary>
	/// Provide terrains so the Manager can be initialized with proper values
	/// </summary>
	/// <param name="terrains">Terrains.</param>
	void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight);
}
