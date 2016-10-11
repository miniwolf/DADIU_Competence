using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class MapGenerator : MonoBehaviour {
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

		public void Generate() {
			var map = PerlinNoise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, offset, seed);

			mapDisplay.DrawMap(map);
		}

		public bool IsAutoUpdate() {
			return autoUpdate;
		}
	}
}

