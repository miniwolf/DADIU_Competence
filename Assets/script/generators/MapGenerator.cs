using System;
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

		[Range(1,6)]
		public int LOD = 1;

		private const int chunkSize = 241;

		public void Generate() {
			var noiseMap = PerlinNoise.GenerateNoiseMap(chunkSize, chunkSize, scale, octaves, persistance, lacunarity, offset, seed);

			var colourMap = new Color[chunkSize * chunkSize];
			for ( int y = 0; y < chunkSize; y++ ) {
				for ( int x = 0; x < chunkSize; x++ ) {
					float curHeight = noiseMap[x,y];
					foreach ( Terrain terrain in regions ) {
						if ( curHeight <= terrain.height ) {
							colourMap[y * chunkSize + x] = terrain.colour;
							break;
						}
					}
				}
			}
			switch ( mode ) {
				case Mode.ColourMap:
					mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, chunkSize, chunkSize));
					break;
				case Mode.NoiseMap:
					mapDisplay.DrawTexture(TextureGenerator.TextureFromHeighMap(noiseMap));
					break;
				case Mode.Mesh:
					var texture = TextureGenerator.TextureFromColourMap(colourMap, chunkSize, chunkSize);
					mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeight, heightCurve, LOD), texture);
					break;
			}
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
}
