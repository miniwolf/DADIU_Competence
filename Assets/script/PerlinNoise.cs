using UnityEngine;

namespace AssemblyCSharp {
	public static class PerlinNoise {
		private const float EPSILON = 0.0001f;
		private static int height, width;

		public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, int seed) {
			var map = new float[width, height];

			var random = new System.Random(seed);
			var octaveOffets = new Vector2[octaves];

			for ( int i = 0; i < octaves; i++ ) {
				float offsetX = random.Next(-100000, 100000) + offset.x;
				float offsetY = random.Next(-100000, 100000) + offset.y;
				octaveOffets[i] = new Vector2(offsetX, offsetY);
			}

			PerlinNoise.height = height;
			PerlinNoise.width = width;

			Mathf.Clamp(scale, EPSILON, float.MaxValue);

			float maxNoise = float.MinValue;
			float minNoise = float.MaxValue;

			float halfWidth = width * .5f;
			float halfHeight = height * .5f;

			for ( int y = 0; y < width; y++ ) {
				for ( int x = 0; x < height; x++ ) {
					float amplitude = 1;
					float frequency = 1;
					float noiseHeight = 0;

					for ( int i = 0; i < octaves; i++ ) {
						// Higher frequency, further apart sample points
						float sampleX = (x - halfHeight) / scale * frequency + octaveOffets[i].x;
						float sampleY = (y - halfWidth) / scale * frequency + octaveOffets[i].y;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // [0,1] -> [-1,1]
						noiseHeight += perlinValue * amplitude;

						amplitude *= persistance; // persistance [0,1]
						frequency *= lacunarity; // lacunarity > 1
					}
					if ( noiseHeight > maxNoise ) {
						maxNoise = noiseHeight;
					}
					if ( noiseHeight < minNoise ) {
						minNoise = noiseHeight;
					}
					map[x,y] = noiseHeight;
				}
			}

			NormalizeNoiseMap(minNoise, maxNoise, map);

			return map;
	  	}

		/// <summary>
		/// Normalizes the noise map. To be between 0 and 1
		/// </summary>
		private static void NormalizeNoiseMap(float minNoise, float maxNoise, float[,] map) {
			for ( int y = 0; y < width; y++ ) {
				for ( int x = 0; x < height; x++ ) {
					map[x,y] = Mathf.InverseLerp(minNoise, maxNoise, map[x,y]);
				}
			}
		}
	}
}