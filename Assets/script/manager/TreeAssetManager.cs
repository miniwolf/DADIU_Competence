using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeAssetManager : MonoBehaviour, StaticAssetManager {

	[Range(1, 99), Tooltip("Tree density. Effects visible after you re-generate map")]
	public int treeDensity = 1;

	private IList used, free;
	private float grassMinHeight, grassMaxHeight;
	private GameObject treeTemplate;

	private int physicsHitUp, physicsHitDown;

	private const string TAG = "TreeClone";

	private Stack<GameObject> trees = new Stack<GameObject>();

	private int cntAllocated, cntRecycled;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void Init(AssemblyCSharp.Terrain[] terrains) {

		cntAllocated = 0;
		cntRecycled = 0;
		physicsHitDown = 0;
		physicsHitUp = 0;

		used = new ArrayList();
		free = new ArrayList();

		for( int i = 0; i < terrains.Length; i++ ) {
			// todo we can use TagConstants here
			// This code assumes that terrains is sorted from min height to max height, there are 2 types of grass and grass is not the first one in the array (otherwise terrains[i-1] fails)
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				grassMinHeight = terrains[i - 1].height; // height of what is before grass
				grassMaxHeight = terrains[i + 1].height; // second grass element
				break;
			}
		}
	}

	public void PlayerPositionUpdated(Vector3 newPos) {

	}

	public void NewPointOfInterest(float normalizedHeight, Vector3 newPos) {
		if( normalizedHeight >= grassMinHeight && normalizedHeight <= grassMaxHeight ) {
			free.Add(newPos);
		}
	}

	public void NewPointOfInterest(float normalizedHeight, int x, int y, AnimationCurve heightCurve, float heightMutliplier) {
//		// Getting centered vertices
		float topLeftX = ( 241 - 1 ) * -.5f; 
		float topLeftZ = ( 241 - 1 ) * .5f; 
		float param = 10f;
		NewPointOfInterest(normalizedHeight, new Vector3(( topLeftX + x ) * param, heightCurve.Evaluate(normalizedHeight) * heightMutliplier, ( topLeftZ - y ) * param));
	}

	/// <summary>
	/// When the map is rendered, calculate the position of the trees and display
	/// </summary>
	public void OnMapRendered() {
		Debug.Log("Start_Render stats: Allocated: " + cntAllocated + ", recycled: " + cntRecycled + ", treeStorageSize: " + trees.Count);

		if( treeTemplate == null ) // renderred for the first time, no trees on the scene
			treeTemplate = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.TREE_TEMPLATE);
		else {
			// recycle existing trees
			GameObject[] existingTrees = GameObject.FindGameObjectsWithTag(TAG);

			if( existingTrees == null || existingTrees.Length == 0 ) { // all trees were most likely removed from scene manually
				trees = new Stack<GameObject>();
			} else
				foreach( GameObject o in existingTrees ) {
//				DestroyImmediate(o);
					o.SetActive(false);
					trees.Push(o);
				}
		}
		Debug.Log("Middle_Render stats: Allocated: " + cntAllocated + ", recycled: " + cntRecycled + ", treeStorageSize: " + trees.Count);

//		trees = new HashSet<GameObject>()
//
//		if( true )
//			return;

		Debug.Log("Free plant points: " + free.Count);

		for( int i = 0; i < free.Count; i += ( 100 - treeDensity ) ) {
			GameObject g = GetObject();
			RandomizeTree(g);

			Vector3 freePos = (Vector3)free[Random.Range(0, free.Count)];
			freePos.x += Random.Range(-1, 1);
			freePos.z += Random.Range(-1, 1);

			RaycastHit hit;

			// sometimes the freePos is wrong - is above or below the mesh
			if( Physics.Raycast(freePos, -Vector3.up, out hit, 100) ) {
				freePos = hit.point;
				physicsHitUp++;
//				g.transform.position = freePos;
			} else if( Physics.Raycast(freePos, -Vector3.down, out hit, 100) ) {
				freePos = hit.point;
				physicsHitDown++;
//			} else {
//								g.transform.position = freePos;
//				g.SetActive(false);
//				trees.Push(g);
			}

			g.transform.position = freePos;




//			Vector3 pos = (Vector3)free[Random.Range(0, free.Count)];
//			GameObject g = GetObject();
//			g.transform.position = pos;
//			g.tag = TAG;
		}

		Debug.Log("End_Render stats: Allocated: " + cntAllocated +
					", recycled: " + cntRecycled +
					", treeStorageSize: " + trees.Count +
					", hitUp: " + physicsHitUp +
					", hitDown: " + physicsHitDown);
	}

	void RandomizeTree(GameObject g) {
		Wasabimole.ProceduralTree.ProceduralTree tree = g.GetComponent<Wasabimole.ProceduralTree.ProceduralTree>();
		if( tree != null ) // only if we exchange tree for cube 
			tree.BranchProbability = Random.Range(0.1f, 0.25f);
	}

	private GameObject GetObject() {
		GameObject g;
		if( trees.Count > 0 ) {
			cntRecycled++;
			g = trees.Pop();
			g.SetActive(true);
		} else {
			cntAllocated++;
//			g = GameObject.CreatePrimitive(PrimitiveType.Cube);
//			g.AddComponent<Rigidbody>();
//			Vector3 gScale = g.transform.localScale;
//			gScale.y *= 5;
//			g.transform.localScale = gScale;

			g = (GameObject)Instantiate(treeTemplate);
			g.tag = TAG;
		}

		return g;
	}
}