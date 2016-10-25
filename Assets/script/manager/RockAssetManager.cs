using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.script;

public class RockAssetManager : MonoBehaviour, StaticAssetManager {


	[Range(1, 9999), Tooltip("Rock density.")]
	public int rockDensity = 1;

	public LayerMask mask;

	private GameObject template;
	private float rockMinHeight, rockMaxHeight;
	private	AnimationCurve animCurve;
	private float meshHeight;
	private List<AssemblyCSharp.Terrain> acceptableTerrains;

	void Start() {
		template = GameObject.FindGameObjectWithTag(TagConstants.ROCK_TEMPLATE);
	}

	public void Init(AssemblyCSharp.Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {

		this.animCurve = animCurve;
		this.meshHeight = meshHeight;
		this.acceptableTerrains = new List<AssemblyCSharp.Terrain>();

		bool interested = false;
		for( int i = 0; i < terrains.Length; i++ ) {
			if( terrains[i].name.ToLower().Contains("grass") ) { 
				interested = true;
				rockMinHeight = terrains[i].height;
			}

			if( interested ) { // add all that are higher than "grass"
				acceptableTerrains.Add(terrains[i]);
			}
		}

		rockMaxHeight = acceptableTerrains[acceptableTerrains.Count -1].height; 
	}

	/// <summary>
	/// When the map chunk is rendered, calculate the position of the trees and display
	/// </summary>
	public void OnMapRendered(AssemblyCSharp.MapData info) {
		IList emptyPositions = new ArrayList();

		int width = info.noiseMap.GetLength(0);
		int height = info.noiseMap.GetLength(1);

		float topLeftX = ( width - 1 ) * -.5f; 
		float topLeftZ = ( height - 1 ) * .5f;

		for( int y = 0; y < height; y++ ) {
			for( int x = 0; x < width; x++ ) {

				float normalizedHeight = info.noiseMap[x, y];
				if( normalizedHeight >= rockMinHeight && normalizedHeight <= rockMaxHeight ) { // we are interested in given height

					// sanity check if the given point is in the correct terrain color
					Color currentColor = info.colourMap[y * width + x];

					foreach( AssemblyCSharp.Terrain t in acceptableTerrains ) {
						if( t.colour == currentColor ) {
							emptyPositions.Add(new Vector3(( topLeftX + x ), 
								animCurve.Evaluate(info.noiseMap[x, y]) * meshHeight, 
								( topLeftZ - y )));
						}
					}
				}
			}
		}

		for( int i = 0; i < emptyPositions.Count; i += ( 10000 - rockDensity ) ) {
			GameObject g = (GameObject)Instantiate(template);

			g.transform.parent = transform;
			g.tag = TagConstants.TREE_INST;

			// randomize rock

			Vector3 freePos = (Vector3)emptyPositions[Random.Range(0, emptyPositions.Count)];
			emptyPositions.Remove(freePos);

			freePos.x += info.offset.x;
			freePos.z += info.offset.y;

			freePos.x += Random.Range(-1, 1);
			freePos.z += Random.Range(-1, 1);

			g.transform.position = freePos;
		}
	}
}
