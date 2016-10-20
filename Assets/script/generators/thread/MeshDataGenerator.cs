using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public static class MeshDataGenerator {
		public static void MeshDataThread(MapData mapData, float meshHeight, AnimationCurve heightCurve, int LOD, Action<MeshData> callback, Queue<MapThreadInfo<MeshData>> infoQueue) {
			MeshData meshData = GenerateData(mapData.noiseMap, meshHeight, heightCurve, LOD);
			lock( infoQueue ) {
				infoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
			}
		}

		public static MeshData GenerateData(float[,] map, float meshHeight, AnimationCurve heightCurve, int LOD) {
			var curve = new AnimationCurve(heightCurve.keys);
			int width = map.GetLength(0);
			int height = map.GetLength(1);

			// Getting centered vertices
			float topLeftX = ( width - 1 ) * -.5f; 
			float topLeftZ = ( height - 1 ) * .5f; // Opposite direction in the z direction

			int meshSimplificationIncrement = ( LOD == 0 ) ? 1 : LOD * 2;
			int vertPerLine = ( width - 1 ) / meshSimplificationIncrement + 1;

			var data = new MeshData(vertPerLine, vertPerLine);
			int vertexIndex = 0;

			for( int y = 0; y < height; y += meshSimplificationIncrement ) {
				for( int x = 0; x < width; x += meshSimplificationIncrement ) {
					data.vertices[vertexIndex] = new Vector3(topLeftX + x, curve.Evaluate(map[x, y]) * meshHeight, topLeftZ - y);
					data.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
				
					if( x < width - 1 && y < height - 1 ) { // ignore right and bottom vertices
						data.AddTriangle(vertexIndex, vertexIndex + vertPerLine + 1, vertexIndex + vertPerLine); // defining the two triangles i, i+1, i+w, i+w+1
						data.AddTriangle(vertexIndex + vertPerLine + 1, vertexIndex, vertexIndex + 1); // Try and draw and example of the square containing the 4 vertices
					}

					vertexIndex++;
				}
			}

			return data;
		}
	}
}

