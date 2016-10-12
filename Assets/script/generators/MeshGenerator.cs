using UnityEngine;
using System.Collections;

public static class MeshGenerator {
	public static MeshData GenerateTerrainMesh(float[,] map, float heightMultiplier, AnimationCurve heightCurve, int LOD) {
		int width = map.GetLength(0);
		int height = map.GetLength(1);

		// Getting centered vertices
		float topLeftX = (width-1)*-.5f; 
		float topLeftZ = (height-1)*.5f; // Opposite direction in the z direction

		int meshSimplificationIncrement = (LOD == 0) ? 0 : LOD * 2;
		int vertPerLine = (width - 1) / meshSimplificationIncrement + 1;

		var data = new MeshData(vertPerLine, vertPerLine);
		int vertexIndex = 0;

		for ( int y = 0; y < height; y += meshSimplificationIncrement ) {
			for ( int x = 0; x < width; x += meshSimplificationIncrement ) {
				data.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(map[x,y]) * heightMultiplier, topLeftZ - y);
				data.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float) height);

				if ( x < width - 1 && y < height - 1 ) { // ignore right and bottom vertices
					data.AddTriangle(vertexIndex,  vertexIndex + vertPerLine + 1, vertexIndex + vertPerLine); // defining the two triangles i, i+1, i+w, i+w+1
					data.AddTriangle(vertexIndex + vertPerLine + 1, vertexIndex, vertexIndex + 1); // Try and draw and example of the square containing the 4 vertices
				}
				vertexIndex++;
			}
		}

		return data;
	}
}