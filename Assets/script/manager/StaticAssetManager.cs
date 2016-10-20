using UnityEngine;
using System.Collections;


/// <summary>
/// Interface that navigates static objects in the scene. May contain some caching mechanism (object pooling)
/// </summary>
public interface StaticAssetManager : MapChangeListener {
	/// <summary>
	/// Provide terrains so the Manager can be initialized with proper values
	/// </summary>
	/// <param name="terrains">Terrains.</param>
	void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight);

	/// <summary>
	/// Mesh generator generated a new point. If navigator instance is interested, it should store this information for later use
	/// </summary>
	/// <param name="location">Location.</param>
	/// <param name="height">Height.</param>
	void NewPointOfInterest(float normalizedHeight, Vector3 newPos);

}
