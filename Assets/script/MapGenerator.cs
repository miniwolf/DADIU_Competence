using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class MapGenerator : MonoBehaviour {
		public enum Mode {
			NoiseMap, ColourMap
		}

		public int width, height;
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

		public bool autoUpdate;
		public MapDisplay mapDisplay;
		public Mode mode;

		public ReorderableList<Terrain> regions;

		public void Generate() {
			var noiseMap = PerlinNoise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, offset, seed);

			var colourMap = new Color[width * height];
			for ( int x = 0; x < height; x++ ) {
				for ( int y = 0; y < width; y++ ) {
					float curHeight = noiseMap[x,y];
					foreach ( Terrain terrain in regions ) {
						if ( curHeight <= terrain.height ) {
							colourMap[y * width + x] = terrain.colour;
							break;
						}
					}
				}
			}
			switch ( mode ) {
				case Mode.ColourMap:
					mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, width, height));
					break;
				case Mode.NoiseMap:
					mapDisplay.DrawTexture(TextureGenerator.TextureFromHeighMap(noiseMap));
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
