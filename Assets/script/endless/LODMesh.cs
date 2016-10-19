using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class LODMesh {
		private Mesh mesh;
		private bool isRequestedMesh;
		private bool hasMesh;
		private int LOD;
		private System.Action updateCallback;

		MapGenerator generator;

		public LODMesh(int LOD, MapGenerator generator, System.Action updateCallback) {
			this.generator = generator;
			this.LOD = LOD;
			this.updateCallback = updateCallback;
		}

		private void OnDataReceived(MeshData data) {
			mesh = data.CreateMesh();
			hasMesh = true;
		}

		public void RequestMesh(MapData data) {
			isRequestedMesh = true;
			generator.RequestMeshData(data, LOD, OnDataReceived);
		}

		public Mesh Mesh {
			get {
				return this.mesh;
			}
		}

		public bool IsRequestedMesh {
			get {
				return this.isRequestedMesh;
			}
		}

		public bool HasMesh {
			get {
				return this.hasMesh;
			}
		}
	}
}

