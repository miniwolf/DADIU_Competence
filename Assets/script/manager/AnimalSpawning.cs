using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;
using Assets.script.components.registers;
using UnityEngine;
using Random = UnityEngine.Random;
using Terrain = AssemblyCSharp.Terrain;

namespace Assets.script.manager {
	public class AnimalSpawning : MonoBehaviour, StaticAssetManager, GameStateManager.GameStateChangeListener, GameStateManager.GameModeChangeListener {
		private Terrain terrainMin, terrainMax;
		private	AnimationCurve animCurve;
		private float meshHeight;

		[Range(1, 9999), Tooltip("Animal density")]
		public int animalDensity = 1;

		private GameObject wolfObject;
		private GameObject deerObject;

		public int wolfRatio = 20;

		private Queue<MapData> mapChunks = new Queue<MapData>();
		private GameStateManager gameManager;

		void Start() {
			gameManager = GameObject.FindGameObjectWithTag(Assets.script.TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();
			gameManager.RegisterGameStateListener(this);
			gameManager.RegisterModeListener(this);
		}

		void Destroy() {
			gameManager.UnRegisterModeListener(this);
			gameManager.UnRegisterGameStateListener(this);
		}

		public void OnMapRendered(MapData info) {
			mapChunks.Enqueue(info);
		}

		private void PlaceAnimals() {
			while(mapChunks.Count > 0) {
				PlaceAnimalOnChunk(mapChunks.Dequeue());
			}
		}

		private void PlaceAnimalOnChunk(MapData info) {
			IList emptyPositions = new List<Vector3>();

			var width = info.noiseMap.GetLength(0);
			var height = info.noiseMap.GetLength(1);

			// this is stolen from MeshDataGenerator script
			var topLeftX = ( width - 1 ) * -.5f;
			var topLeftZ = ( height - 1 ) * .5f;

			// check all points from generated chunk
			for( var y = 0; y < height; y++ ) {
				for( var x = 0; x < width; x++ ) {

					var normalizedHeight = info.noiseMap[x, y];
					if ( !(normalizedHeight >= terrainMin.height) || !(normalizedHeight <= terrainMax.height) ) {
						continue;
					}
					// we are interested in given height

					// sanity check if the given point is in the correct terrain color
					var currentColor = info.colourMap[y * width + x];
					if( currentColor == terrainMin.colour || currentColor == terrainMax.colour ) {
						emptyPositions.Add(new Vector3(topLeftX + x,
							animCurve.Evaluate(info.noiseMap[x, y]) * meshHeight, topLeftZ - y));
					}
				}
			}

			Vector3[] freePos = {Vector3.zero};
			var mySwitch = new Dictionary<Func<int, bool>, Action> {
				{ x => x < wolfRatio, () => {
						var wolf = (GameObject) Instantiate(wolfObject, freePos[0], Quaternion.identity);
						wolf.transform.parent = transform;
					} },
				{ x => x < 100, () => {
						var deer = (GameObject) Instantiate(deerObject, freePos[0], Quaternion.identity);
						deer.transform.parent = transform;
					} }
			};
			for ( var i = 0; i < emptyPositions.Count; i += 10000 - animalDensity ) {
				freePos[0] = (Vector3)emptyPositions[Random.Range(0, emptyPositions.Count)];
				emptyPositions.Remove(freePos[0]);

				// apply terrain chunk offset
				freePos[0].x += info.offset.x;
				freePos[0].z += info.offset.y;

				// add a bit of randomness, so objects are not planted on the grid
				freePos[0].x += Random.Range(-1, 1);
				freePos[0].z += Random.Range(-1, 1);
				freePos[0].y = 10;
				mySwitch.First(sw => sw.Key(Random.Range(0, 100))).Value();
			}
			InjectionRegister.ReDo();
		}


		public void Init(Terrain[] terrains, AnimationCurve animCurve, float meshHeight) {
			this.animCurve = animCurve;
			this.meshHeight = meshHeight;

			wolfObject = GameObject.FindGameObjectWithTag(TagConstants.WOLF);
			deerObject = GameObject.FindGameObjectWithTag(TagConstants.DEER);

			for( var i = 0; i < terrains.Length; i++ ) {
				// todo we can use TagConstants here
				// This code assumes that terrains is sorted from min height to max height, there are 2 types of grass and grass is not the first one in the array (otherwise terrains[i-1] fails)
				if ( !terrains[i].name.ToLower().Contains("grass") ) {
					continue;
				}
				terrainMin = terrains[i]; // height of the first grass
				terrainMax = terrains[i + 1]; // second grass element
				break;
			}
		}

		public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {
			if( newState == GameStateManager.GameState.Playing )
				PlaceAnimals();				
		}

		public void OnGameModeChanged(GameStateManager.GameMode newMode) {
			switch ( newMode ) {
				case GameStateManager.GameMode.Score:
					wolfRatio /= 3;
					break;
				case GameStateManager.GameMode.Survival:
					wolfRatio *= 3;
					break;
			}
		}
	}
}