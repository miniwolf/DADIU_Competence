using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public static class MapDataGenerator {
		public static void MapDataThread(Action<MapData> callback, Queue<MapThreadInfo<MapData>> infoQueue, MapDataInfo info) {
			MapData data = GenerateData(info);
			lock (infoQueue) {
				infoQueue.Enqueue(new MapThreadInfo<MapData>(callback, data));
			}
		}

		public static MapData GenerateData(MapDataInfo info) {
			var noiseMap = PerlinNoise.GenerateNoiseMap(info);

			var colourMap = new Color[info.chunkSize * info.chunkSize];
			for ( int y = 0; y < info.chunkSize; y++ ) {
				for ( int x = 0; x < info.chunkSize; x++ ) {
					float curHeight = noiseMap[x,y];
					foreach ( Terrain terrain in info.regions ) {
						if ( curHeight < terrain.height ) {
							break;
						}
						colourMap[y * info.chunkSize + x] = terrain.colour;
					}
				}
			}
			return new MapData(noiseMap, colourMap, info.offset);
		}
	}

	public struct MapDataInfo {
		public Vector2 offset;
		public int chunkSize;
		public int seed;
		public int octaves;
		public float persistance;
		public float lacunarity;
		public float scale;
		public Terrain[] regions;
		public PerlinNoise.NormalizeMode mode;

		public MapDataInfo(Vector2 center, Vector2 offset, int chunkSize, int seed, int octaves, float persistance, float lacunarity, float scale, Terrain[] regions, PerlinNoise.NormalizeMode mode) {
			this.offset = center + offset;
			this.chunkSize = chunkSize;
			this.persistance = persistance;
			this.lacunarity = lacunarity;
			this.scale = scale;
			this.seed = seed;
			this.regions = regions;
			this.mode = mode;
			this.octaves = octaves;
		}
	}
}
