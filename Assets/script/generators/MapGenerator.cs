using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class MapGenerator : MonoBehaviour {
		public enum Mode {
			Mesh, NoiseMap, ColourMap
		}

		[Tooltip("To be described")]
		public int octaves;
		[Tooltip("Will set the random noise to be a specified value. Create unique noise")]
		public int seed;
		[Range(0,100)]
		public float scale;
		[Range(0,1), Tooltip("Will descale the amplitude")]
		public float persistance;
		[Range(1,10), Tooltip("Modifies the frequency")]
		public float lacunarity;

		public Vector2 offset;
		public float meshHeight;

		public bool autoUpdate;
		public MapDisplay mapDisplay;
		public Mode mode;

		public AnimationCurve heightCurve;
		public Terrain[] regions;

		Queue<MapThreadInfo<MapData>> mapThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
		Queue<MapThreadInfo<MeshData>> meshThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

		public const int chunkSize = 241; // actual mesh is 1 less than this

		[Range(1,6)]
		public int editorLOD = 1;

		void Update() {
			if ( mapThreadInfoQueue.Count > 0 ) {
				for ( int i = 0; i < mapThreadInfoQueue.Count; i++ ) {
					var data = mapThreadInfoQueue.Dequeue();
					data.callback(data.parameter);
				}
			}

			if ( meshThreadInfoQueue.Count > 0 ) {
				for ( int i = 0; i < meshThreadInfoQueue.Count; i++ ) {
					var data = meshThreadInfoQueue.Dequeue();
					data.callback(data.parameter);
				}
			}
		}

		/*public void DrawInEditor() {
			//MapData data = GenerateData();

			switch ( mode ) {
			case Mode.ColourMap:
				mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(data.colourMap, chunkSize, chunkSize));
				break;
			case Mode.NoiseMap:
				mapDisplay.DrawTexture(TextureGenerator.TextureFromHeighMap(data.noiseMap));
				break;
			case Mode.Mesh:
				var texture = TextureGenerator.TextureFromColourMap(data.colourMap, chunkSize, chunkSize);
				//mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(data.noiseMap, meshHeight, heightCurve, LOD), texture);
				break;
			}
		}*/

		public void RequestTerrainData(Action<MapData> callback, Vector2 center) {
			var generator = new MapDataGenerator(chunkSize, scale, octaves, persistance, lacunarity, offset, seed, regions);
			ThreadStart runnable = delegate {
				generator.MapDataThread(callback, mapThreadInfoQueue, center);
			};

			new Thread(runnable).Start();
		}

		public void RequestMeshData(MapData data, int lod, Action<MeshData> callback) {
			var generator = new MeshDataGenerator(meshHeight, heightCurve, lod);
			ThreadStart runnable = delegate {
				generator.MeshDataThread(data, callback, meshThreadInfoQueue);
			};

			new Thread(runnable).Start();
		}

		public bool IsAutoUpdate() {
			return autoUpdate;
		}

	}

	[System.Serializable]
	public struct Terrain {
		public string name;
		public float height;
		public Color colour;
	}

	public struct MapData {
		public readonly float[,] noiseMap;
		public readonly Color[] colourMap;

		public MapData(float[,] noiseMap, Color[] colourMap) {
			this.noiseMap = noiseMap;
			this.colourMap = colourMap;
		}
	}

	public struct MapThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo(Action<T> callback, T parameter) {
			this.callback = callback;
			this.parameter = parameter;
		}
	}
}
