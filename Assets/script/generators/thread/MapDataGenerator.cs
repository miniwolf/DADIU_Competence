using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class MapDataGenerator {
		private int seed;
		private int octaves;
		private int chunkSize;

		private float lacunarity;
		private float persistance;
		private float scale;

		private Vector2 offset;
		private Terrain[] regions;

		public MapDataGenerator(int chunkSize, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, int seed, Terrain[] regions) {
			this.regions = regions;
			this.chunkSize = chunkSize;
			this.scale = scale;
			this.octaves = octaves;
			this.persistance = persistance;
			this.lacunarity = lacunarity;
			this.offset = offset;
			this.seed = seed;
		}

		public void MapDataThread(Action<MapData> callback, Queue<MapThreadInfo<MapData>> infoQueue) {
			MapData data = GenerateData();
			lock (infoQueue) {
				infoQueue.Enqueue(new MapThreadInfo<MapData>(callback, data));
			}
		}

		private MapData GenerateData() {
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
			return new MapData(noiseMap, colourMap);
		}
	}
}

