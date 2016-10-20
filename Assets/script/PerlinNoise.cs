using UnityEngine;

namespace AssemblyCSharp {
	public static class PerlinNoise {
		private const float EPSILON = 0.0001f;

		public enum NormalizeMode { Local, Global };

		public static float[,] GenerateNoiseMap(MapDataInfo info) {
			var map = new float[info.chunkSize, info.chunkSize];

			var random = new System.Random(info.seed);
			var octaveOffets = new Vector2[info.octaves];

			float maxHeight = 0;
			float amplitude = 1;
			float frequency = 1;

			for ( int i = 0; i < info.octaves; i++ ) {
				float offsetX = random.Next(-100000, 100000) + info.offset.x;
				float offsetY = random.Next(-100000, 100000) - info.offset.y;
				octaveOffets[i] = new Vector2(offsetX, offsetY);

				maxHeight += amplitude;
				amplitude *= info.persistance;
			}

			Mathf.Clamp(info.scale, EPSILON, float.MaxValue);

			float maxNoise = float.MinValue;
			float minNoise = float.MaxValue;

			float halfWidth = info.chunkSize * .5f;
			float halfHeight = info.chunkSize* .5f;

			for ( int y = 0; y < info.chunkSize; y++ ) {
				for ( int x = 0; x < info.chunkSize; x++ ) {
					float noiseHeight = 0;
					amplitude = 1;
					frequency = 1;

					for ( int i = 0; i < info.octaves; i++ ) {
						// Higher frequency, further apart sample points
						float sampleX = (x - halfHeight + octaveOffets[i].x) / info.scale * frequency;
						float sampleY = (y - halfWidth + octaveOffets[i].y) / info.scale * frequency;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // [0,1] -> [-1,1]
						noiseHeight += perlinValue * amplitude;

						amplitude *= info.persistance; // persistance [0,1]
						frequency *= info.lacunarity; // lacunarity > 1
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

			NormalizeNoiseMap(info.chunkSize, minNoise, maxNoise, map, info.mode, maxHeight);

			return map;
	  	}

		/// <summary>
		/// Normalizes the noise map. To be between 0 and 1
		/// </summary>
		private static void NormalizeNoiseMap(float chunkSize, float localMinNoise, float localMaxNoise, float[,] map, NormalizeMode mode, float globalMaxNoise) {
			for ( int y = 0; y < chunkSize; y++ ) {
				for ( int x = 0; x < chunkSize; x++ ) {
					map[x, y] = ( mode == NormalizeMode.Global ) 
						? Mathf.Clamp((map[x,y] + 1) / (globalMaxNoise / .7f), 0, int.MaxValue)
						: Mathf.InverseLerp(localMinNoise, localMaxNoise, map[x, y]);
				}
			}
		}
	}
}