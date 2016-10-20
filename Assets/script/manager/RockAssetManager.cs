using UnityEngine;
using System.Collections;

public class RockAssetManager : MonoBehaviour, StaticAssetManager {


	[Range(1, 9999), Tooltip("Rock density.")]
	public int rockDensity = 1;

	public LayerMask mask;

	private IList free = new ArrayList();
	private float grassMinHeight, grassMaxHeight;
	private GameObject treeTemplate;
	private	AnimationCurve animCurve;
	private float meshHeight;

	void Start() {
		treeTemplate = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.ROCK_TEMPLATE);
	}

	public void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {

		this.animCurve = animCurve;
		this.meshHeight = meshHeight;
		free = new ArrayList();

		for( int i = 0; i < terrains.Length; i++ ) {
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				grassMinHeight = terrains[i].height; 
				break;
			}
		}

		grassMaxHeight = terrains[terrains.Length - 1].height; 
		Debug.Log("grassMinHeight: " + grassMinHeight + ", grassMaxHeight: " + grassMaxHeight);
	}

	public void NewPointOfInterest(float normalizedHeight, Vector3 newPos) {
		if( normalizedHeight >= grassMinHeight && normalizedHeight <= grassMaxHeight ) {
			free.Add(newPos);
		}
	}

	/// <summary>
	/// When the map chunk is rendered, calculate the position of the trees and display
	/// </summary>
	public void OnMapRendered(AssemblyCSharp.MapData info) {
		int width = info.noiseMap.GetLength(0);
		int height = info.noiseMap.GetLength(1);

		float topLeftX = ( width - 1 ) * -.5f; 
		float topLeftZ = ( height - 1 ) * .5f;

		for( int y = 0; y < height; y++ ) {
			for( int x = 0; x < width; x++ ) {
				NewPointOfInterest(info.noiseMap[x, y], 
					new Vector3(( topLeftX + x ), 
						animCurve.Evaluate(info.noiseMap[x, y]) * meshHeight, 
						( topLeftZ - y ))
				);
			}
		}

		for( int i = 0; i < free.Count; i += ( 10000 - rockDensity ) ) {
			GameObject g = (GameObject)Instantiate(treeTemplate);

			g.transform.parent = transform;
			g.tag = AssemblyCSharp.TagConstants.TREE_INST;

			// randomize rock

			Vector3 freePos = (Vector3)free[Random.Range(0, free.Count)];

			freePos.x += info.offset.x;
			freePos.z += info.offset.y;

			freePos.x += Random.Range(-1, 1);
			freePos.z += Random.Range(-1, 1);

//			RaycastHit hit;
//			// sometimes the freePos is wrong - is above or below the mesh
//			if( Physics.Raycast(freePos, Vector3.up, out hit, 100, mask) ) {
//				freePos = hit.point;
//			} else if( Physics.Raycast(freePos, Vector3.down, out hit, 100, mask) ) {
//				freePos = hit.point;
//			}

			g.transform.position = freePos;
		}
	}
}
