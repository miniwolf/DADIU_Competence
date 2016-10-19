using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class TerrainChunk {
		private GameObject mesh;
		private Bounds bounds;
		private MapGenerator generator;
		private MeshRenderer renderer;
		private MeshFilter filter;
		private LODInfo[] LODlevels;
		private LODMesh[] LODmeshes;
		private MapData data;
		private bool receivedMapData;
		private int prevLODIdx = -1;

		public TerrainChunk(Vector2 coord, int size, Transform parent, MapGenerator generator, Material material, LODInfo[] LODlevels) {
			this.LODlevels = LODlevels;
			this.generator = generator;

			LODmeshes = new LODMesh[LODlevels.Length];
			for ( int i = 0; i < LODlevels.Length; i++ ) {
				LODmeshes[i] = new LODMesh(LODlevels[i].LOD, generator);
			}

			var pos = coord * size;
			bounds = new Bounds(pos, Vector2.one * size);
			var positionIn3D = new Vector3(pos.x, 0, pos.y);

			SetupMesh(parent, material, positionIn3D);
			SetVisible(false);

			generator.RequestTerrainData(OnMapReceived);
		}

		void SetupMesh(Transform parent, Material material, Vector3 positionIn3D) {
			mesh = new GameObject("Terrain chunk");
			renderer = mesh.AddComponent<MeshRenderer>();
			renderer.material = material;
			filter = mesh.AddComponent<MeshFilter>();
			mesh.transform.position = positionIn3D;
			mesh.transform.parent = parent;
		}

		/// <summary>
		/// Find closest point on perimeter, enable if inside distance
		/// </summary>
		public void UpdateChunk(Vector2 viewerPos, float maxViewDst) {
			if ( !receivedMapData ) {
				return;
			}

			float viewDist = Mathf.Sqrt(bounds.SqrDistance(viewerPos));
			bool visible = viewDist <= maxViewDst;

			if ( visible ) {
				int idx = 0;
				for ( int i = 0; i < LODlevels.Length - 1; i++ ) {
					if ( viewDist > LODlevels[i].visibleThreshold ) {
						idx = i + 1;
					} else {
						break;
					}
				}

				UpdateMeshLOD(idx);
			}
			SetVisible(visible);
		}

		private void UpdateMeshLOD(int idx) {
			if ( idx != prevLODIdx ) {
				var lodMesh = LODmeshes[idx];
				if ( lodMesh.hasMesh ) {
					prevLODIdx = idx;
					filter.mesh = lodMesh.mesh;
				} else if ( !lodMesh.isRequestedMesh ) {
					lodMesh.RequestMesh(data);
				}
			}
		}

		public void OnMapReceived(MapData data) {
			this.data = data;
			receivedMapData = true;
			//generator.RequestMeshData(data, OnMeshReceived);
		}

		public void OnMeshReceived(MeshData data) {
			//filter.mesh = data.CreateMesh();
		}

		public void SetVisible(bool visible) {
			mesh.SetActive(visible);
		}

		public bool IsVisible() {
			return mesh.activeSelf;
		}
	}
}

