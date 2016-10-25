using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.script;

namespace AssemblyCSharp {
	public class MapGenerator : MonoBehaviour {
		public enum Mode {
			Mesh,
			NoiseMap,
			ColourMap
		}

		[Tooltip("To be described")]
		public int octaves;
		[Tooltip("Will set the random noise to be a specified value. Create unique noise")]
		public int seed;
		public const int chunkSize = 241;
		// actual mesh is 1 less than this
		[Range(0, 100)]
		public float scale;
		[Range(0, 1), Tooltip("Will descale the amplitude")]
		public float persistance;
		[Range(1, 10), Tooltip("Modifies the frequency")]
		public float lacunarity;

		[Tooltip("Changes the way we set the normalization of heights in our meshes")]
		public PerlinNoise.NormalizeMode normalizeMode;

		public Vector2 offset;
		public float meshHeight;

		public bool autoUpdate;
		public MapDisplay mapDisplay;
		public Mode mode;

		public AnimationCurve heightCurve;
		public Terrain[] regions;

		private readonly Queue<MapThreadInfo<MapData>> mapThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
		private readonly Queue<MapThreadInfo<MeshData>> meshThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

		private StaticAssetManager[] assetManagers;
		private readonly List<MapChangeListener> mapChangeListeners = new List<MapChangeListener>();

		[Range(1, 6)]
		public int editorLOD = 1;

		protected void Awake() {
			InitAssetManagers();
		}

		protected void Update() {
			if ( mapThreadInfoQueue.Count > 0 ) {
				for ( int i = 0; i < mapThreadInfoQueue.Count; i++ ) {
					var data = mapThreadInfoQueue.Dequeue();
					data.callback(data.parameter); // 
					foreach( MapChangeListener l in mapChangeListeners ) {
						l.OnMapRendered(data.parameter); 
					}
				}
			}

			if ( meshThreadInfoQueue.Count > 0 ) {
				for ( int i = 0; i < meshThreadInfoQueue.Count; i++ ) {
					var data = meshThreadInfoQueue.Dequeue();
					data.callback(data.parameter); // TerrainChunk.OnMapReceived
				}
			}
		}

		public void DrawInEditor() {
			var data = MapDataGenerator.GenerateData(new MapDataInfo(Vector2.zero, offset, chunkSize, seed, octaves, persistance, lacunarity, scale, regions, normalizeMode));
			var texture = TextureGenerator.TextureFromColourMap(data.colourMap, chunkSize, chunkSize);

			switch( mode ) {
			case Mode.ColourMap:
				mapDisplay.DrawTexture(texture);
				break;
			case Mode.NoiseMap:
				mapDisplay.DrawTexture(TextureGenerator.TextureFromHeighMap(data.noiseMap));
				break;
			case Mode.Mesh:
				var meshData = MeshDataGenerator.GenerateData(data.noiseMap, meshHeight, heightCurve, editorLOD);
				mapDisplay.DrawMesh(meshData, texture);
				break;
			}
		}

		public void RequestTerrainData(Action<MapData> callback, Vector2 center) {
			ThreadStart runnable = delegate {
				MapDataGenerator.MapDataThread(callback, mapThreadInfoQueue, new MapDataInfo(center, offset, chunkSize, seed, octaves, persistance, lacunarity, scale, regions, normalizeMode));
			};

			new Thread(runnable).Start();
		}

		public void RequestMeshData(MapData data, int lod, Action<MeshData> callback) {
			ThreadStart runnable = delegate {
				MeshDataGenerator.MeshDataThread(data, meshHeight, heightCurve, lod, callback, meshThreadInfoQueue);
			};

			new Thread(runnable).Start();
		}

		public bool IsAutoUpdate() {
			return autoUpdate;
		}

		private void InitAssetManagers() {
			assetManagers = GameObject.FindGameObjectWithTag(TagConstants.ASSET_MANAGER).GetComponentsInChildren<StaticAssetManager>();

			mapChangeListeners.AddRange(assetManagers);
			foreach( StaticAssetManager manager in assetManagers ) {
				manager.Init(regions, heightCurve, meshHeight);
			}
		}
	}

	[Serializable]
	public struct Terrain {
		public string name;
		public float height;
		public Color colour;
	}

	public struct MapData {
		public readonly float[,] noiseMap;
		public readonly Color[] colourMap;
		public readonly Vector2 offset;

		public MapData (float[,] noiseMap, Color[] colourMap, Vector2 offset) {
			this.noiseMap = noiseMap;
			this.colourMap = colourMap;
			this.offset = offset;
		}
	}

	public struct MapThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo (Action<T> callback, T parameter) {
			this.callback = callback;
			this.parameter = parameter;
		}
	}

//	public struct AssetInfo {
//		public Vector3 point;
//		public float normalizedHeight;
//	}

}
