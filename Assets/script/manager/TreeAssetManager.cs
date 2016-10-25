using UnityEngine;
using System.Collections;
using Assets.script;

public class TreeAssetManager : MonoBehaviour, StaticAssetManager {
	[Range(1, 9999), Tooltip("Tree density")]
	public int treeDensity = 1;

	public bool useCubes = false;

	private GameObject treeTemplate;
	private	AnimationCurve animCurve;
	private float meshHeight;
	private AssemblyCSharp.Terrain terrainMin, terrainMax;

	void Start() {
		treeTemplate = GameObject.FindGameObjectWithTag(TagConstants.TREE_TEMPLATE);
	}

	public void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {

		this.animCurve = animCurve;
		this.meshHeight = meshHeight;

		for( int i = 0; i < terrains.Length; i++ ) {
			// todo we can use TagConstants here
			// This code assumes that terrains is sorted from min height to max height, there are 2 types of grass and grass is not the first one in the array (otherwise terrains[i-1] fails)
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				terrainMin = terrains[i]; // height of the first grass
				terrainMax = terrains[i + 1]; // second grass element
				break;
			}
		}

//		Debug.Log("Grass heights: min: " + terrainMin.height + ", max: " + terrainMax.height);
	}

	/// <summary>
	/// When the map chunk is rendered, calculate the position of the trees and display
	/// </summary>
	public void OnMapRendered(AssemblyCSharp.MapData info) {

		IList emptyPositions = new ArrayList();

		int width = info.noiseMap.GetLength(0);
		int height = info.noiseMap.GetLength(1);

		// this is stolen from MeshDataGenerator script
		float topLeftX = ( width - 1 ) * -.5f; 
		float topLeftZ = ( height - 1 ) * .5f;

		// check all points from generated chunk 
		for( int y = 0; y < height; y++ ) {
			for( int x = 0; x < width; x++ ) {

				float normalizedHeight = info.noiseMap[x, y];
				if( normalizedHeight >= terrainMin.height && normalizedHeight <= terrainMax.height ) { // we are interested in given height

					// sanity check if the given point is in the correct terrain color
					Color currentColor = info.colourMap[y * width + x];
					if( currentColor == terrainMin.colour || currentColor == terrainMax.colour ) { 
						emptyPositions.Add(new Vector3(( topLeftX + x ), 
							animCurve.Evaluate(info.noiseMap[x, y]) * meshHeight, 
							( topLeftZ - y )));
					}
				}
			}
		}
			
		for( int i = 0; i < emptyPositions.Count; i += ( 10000 - treeDensity ) ) {
			GameObject g = GetObject();
			RandomizeTree(g);

			Vector3 freePos = (Vector3)emptyPositions[Random.Range(0, emptyPositions.Count)];
			emptyPositions.Remove(freePos);

			// apply terrain chunk offset
			freePos.x += info.offset.x; 
			freePos.z += info.offset.y;

			// add a bit of randomness, so objects are not planted on the grid
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
			var gScale = g.transform.localScale;
			gScale.y *= 15;
			g.transform.localScale = gScale;
		} else {
			g = Instantiate(treeTemplate);
		}
			
		g.transform.parent = transform;
		g.tag = TagConstants.TREE_INST;

		return g;
	}
}