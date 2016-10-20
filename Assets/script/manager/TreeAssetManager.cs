using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeAssetManager : MonoBehaviour, StaticAssetManager {

	[Range(1, 9999), Tooltip("Tree density. Effects visible after you re-generate map")]
	public int treeDensity = 1;

	private IList used = new ArrayList();
	private IList free = new ArrayList();
	private float grassMinHeight, grassMaxHeight;
	private GameObject treeTemplate;

	private int physicsHitUp, physicsHitDown;

	private const string TAG = "TreeClone";

	private Stack<GameObject> trees = new Stack<GameObject>();

	private int cntAllocated, cntRecycled;

	AnimationCurve animCurve;
	float meshHeight;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {

		this.animCurve = animCurve;
		this.meshHeight = meshHeight;

		used = new ArrayList();
		free = new ArrayList();

		for( int i = 0; i < terrains.Length; i++ ) {
			// todo we can use TagConstants here
			// This code assumes that terrains is sorted from min height to max height, there are 2 types of grass and grass is not the first one in the array (otherwise terrains[i-1] fails)
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				grassMinHeight = terrains[i].height; 
				grassMaxHeight = terrains[i + 1].height; // second grass element

				Debug.Log("grassMinHeight: " + grassMinHeight + ", grassMaxHeight: " + grassMaxHeight);

				break;
			}
		}
	}

	public void PlayerPositionUpdated(Vector3 newPos) {

	}

	//	int attempts = 0, hits = 0;

	public void NewPointOfInterest(float normalizedHeight, Vector3 newPos) {
		if( normalizedHeight >= grassMinHeight && normalizedHeight <= grassMaxHeight ) {
			free.Add(newPos);
		}
	}

	public void NewPointOfInterest(float normalizedHeight, int x, int y, AnimationCurve heightCurve, float heightMutliplier) {
//		// Getting centered vertices
//		float topLeftX = ( 241 - 1 ) * -.5f; 
//		float topLeftZ = ( 241 - 1 ) * .5f; 
//		float param = 10f;
//		NewPointOfInterest(normalizedHeight, new Vector3(( topLeftX + x ) * param, heightCurve.Evaluate(normalizedHeight) * heightMutliplier, ( topLeftZ - y ) * param));
	}

	/// <summary>
	/// When the map is rendered, calculate the position of the trees and display
	/// </summary>
	public void OnMapRendered(AssemblyCSharp.MapData info) {
		Debug.Log("OnMapRendered, offset: " + info.offset +
		", info.colourMap: " + info.colourMap.GetLength(0) +
		", info.noiseMap: " + info.noiseMap.GetLength(0));

		int width = info.noiseMap.GetLength(0);
		int height = info.noiseMap.GetLength(1);

		// Getting centered vertices
		float topLeftX = ( width - 1 ) * -.5f; 
		float topLeftZ = ( height - 1 ) * .5f; // Opposite direction in the z direction
		float param = 1f;
		var r = new System.Random();

		for( int y = 0; y < height; y++ ) {
			for( int x = 0; x < width; x++ ) {
//				if( r.Next(100) == 0 )
					NewPointOfInterest(info.noiseMap[x, y], 
						new Vector3(( topLeftX + x ) * param, 
							animCurve.Evaluate(info.noiseMap[x, y]) * meshHeight, 
							( topLeftZ - y ) * param)
					);
			}
		}

//		Debug.Log("Start_Render stats: Allocated: " + cntAllocated + ", recycled: " + cntRecycled + ", treeStorageSize: " + trees.Count);

		if( treeTemplate == null ) // renderred for the first time, no trees on the scene
			treeTemplate = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.TREE_TEMPLATE);
//		Debug.Log("Middle_Render stats: Allocated: " + cntAllocated + ", recycled: " + cntRecycled + ", treeStorageSize: " + trees.Count);
//		Debug.Log("Free plant points: " + free.Count);

		for( int i = 0; i < free.Count; i += ( 10000 - treeDensity ) ) {
			GameObject g = GetObject();
			RandomizeTree(g);

			Vector3 freePos = (Vector3)free[Random.Range(0, free.Count)];

			freePos.x += info.offset.x;
			freePos.z += info.offset.y;

			freePos.x += Random.Range(-1, 1);
			freePos.z += Random.Range(-1, 1);

			RaycastHit hit;

			// sometimes the freePos is wrong - is above or below the mesh
//			if( Physics.Raycast(freePos, -Vector3.up, out hit, 1000) ) {
//				freePos = hit.point;
//				physicsHitUp++;
////				g.transform.position = freePos;
//			} else if( Physics.Raycast(freePos, -Vector3.down, out hit, 1000) ) {
//				freePos = hit.point;
//				physicsHitDown++;
//			} else {
//								g.transform.position = freePos;
//				g.SetActive(false);
//				trees.Push(g);
//			}

			g.transform.position = freePos;
		}

//		Debug.Log("End_Render stats: Allocated: " + cntAllocated +
//		", recycled: " + cntRecycled +
//		", treeStorageSize: " + trees.Count +
//		", hitUp: " + physicsHitUp +
//		", hitDown: " + physicsHitDown);
	}

	void RandomizeTree(GameObject g) {
		Wasabimole.ProceduralTree.ProceduralTree tree = g.GetComponent<Wasabimole.ProceduralTree.ProceduralTree>();
		if( tree != null ) // only if we exchange tree for cube 
			tree.BranchProbability = Random.Range(0.1f, 0.25f);
	}

	private GameObject GetObject() {
		GameObject g;
		if( trees.Count > 0 ) {
			g = trees.Pop();
			g.SetActive(true);
		} else {
//			g = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			Vector3 gScale = g.transform.localScale;
//			gScale.y *= 5;
//			g.transform.localScale = gScale;

			g = (GameObject)Instantiate(treeTemplate);

			g.transform.parent = transform;
			g.tag = TAG;
		}

		return g;
	}
}