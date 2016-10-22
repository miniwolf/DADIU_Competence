using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeAssetManager : MonoBehaviour, StaticAssetManager {

	[Range(1, 9999), Tooltip("Tree density")]
	public int treeDensity = 1;

	public bool useCubes = false;

	private IList free = new ArrayList();
	private float grassMinHeight, grassMaxHeight;
	private GameObject treeTemplate;
	private	AnimationCurve animCurve;
	private float meshHeight;

	void Start() {
		treeTemplate = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.TREE_TEMPLATE);
	}

	public void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {

		this.animCurve = animCurve;
		this.meshHeight = meshHeight;
		free = new ArrayList();

		for( int i = 0; i < terrains.Length; i++ ) {
			// todo we can use TagConstants here
			// This code assumes that terrains is sorted from min height to max height, there are 2 types of grass and grass is not the first one in the array (otherwise terrains[i-1] fails)
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				grassMinHeight = terrains[i].height; // height of what is before grass
				grassMaxHeight = terrains[i + 1].height; // second grass element
				break;
			}
		}
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
			
		for( int i = 0; i < free.Count; i += ( 10000 - treeDensity ) ) {
			GameObject g = GetObject();
			RandomizeTree(g);

			Vector3 freePos = (Vector3)free[Random.Range(0, free.Count)];

			freePos.x += info.offset.x;
			freePos.z += info.offset.y;

			freePos.x += Random.Range(-1, 1);
			freePos.z += Random.Range(-1, 1);

			g.transform.position = freePos;
		}
	}

	void RandomizeTree(GameObject g) {
		if( useCubes )
			return;
		
		Wasabimole.ProceduralTree.ProceduralTree tree = g.GetComponent<Wasabimole.ProceduralTree.ProceduralTree>();
		tree.BranchProbability = Random.Range(0.1f, 0.25f);
		tree.MinimumRadius = Random.Range(0.01f, 0.04f);
		tree.GenerateTree();
	}

	private GameObject GetObject() {
		GameObject g;

		if( useCubes ) {
			g = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Vector3 gScale = g.transform.localScale;
			gScale.y *= 5;
			g.transform.localScale = gScale;
//			g.AddComponent<Rigidbody>();
//			g.AddComponent<MeshCollider>();
		} else {
			g = (GameObject)Instantiate(treeTemplate);
		}


		g.transform.parent = transform;
		g.tag = AssemblyCSharp.TagConstants.TREE_INST;

		return g;
	}
}