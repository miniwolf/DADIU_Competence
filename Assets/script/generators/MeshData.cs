using UnityEngine;
using System.Collections;

public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int index;

	public MeshData(int width, int height) {
		vertices = new Vector3[width * height];
		uvs = new Vector2[width * height];
		triangles = new int[(width-1)*(height-1)*2*3]; // number of triangles in the map, number of squares * 2 triangles * 3 vertices
	}

	public void AddTriangle(int a, int b, int c) {
		triangles[index] = a;
		triangles[index + 1] = b;
		triangles[index + 2] = c;
		index += 3;
	}

	public Mesh CreateMesh() {
		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		mesh.RecalculateNormals();
		return mesh;
	}
}
