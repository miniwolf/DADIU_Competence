using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class LODMesh {
		public Mesh mesh;
		public bool isRequestedMesh;
		public bool hasMesh;
		int LOD;

		MapGenerator generator;

		public LODMesh(int LOD, MapGenerator generator) {
			this.generator = generator;
			this.LOD = LOD;
		}

		private void OnDataReceived(MeshData data) {
			mesh = data.CreateMesh();
			hasMesh = true;
		}

		public void RequestMesh(MapData data) {
			isRequestedMesh = true;
			generator.RequestMeshData(data, LOD, OnDataReceived);
		}
	}
}

