using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class InfinityTerrain : MonoBehaviour {
		public LODInfo[] detailLevels;
		public static float maxViewDst = 450;
		public static Vector2 viewerPos;

		public Material mapMat;

		private int chunkSize;
		private int chunksVisible;

		private Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
		private List<TerrainChunk> lastTerrainChunks = new List<TerrainChunk>();

		private Transform viewer;
		private MapGenerator generator;

		void Start() {
			viewer = GameObject.FindGameObjectWithTag(TagConstants.PLAYER).transform;
			generator = GameObject.FindGameObjectWithTag(TagConstants.MAPGENERATOR).GetComponent<MapGenerator>();
			maxViewDst = detailLevels[detailLevels.Length - 1].visibleThreshold;
			chunkSize = MapGenerator.chunkSize - 1;
			chunksVisible = Mathf.RoundToInt(maxViewDst / chunkSize);
		}

		void Update() {
			viewerPos = new Vector2(viewer.position.x, viewer.position.z);
			UpdateChunks();
		}

		void UpdateChunks() {
			HideChunks();
			lastTerrainChunks.Clear();

			int currentX = Mathf.RoundToInt(viewerPos.x / chunkSize);
			int currentY = Mathf.RoundToInt(viewerPos.y / chunkSize);

			for ( int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++ ) {
				for ( int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++ ) {
					Vector2 viewedCoord = new Vector2(currentX + xOffset, currentY + yOffset);

					if ( terrainChunks.ContainsKey(viewedCoord) ) {
						// Prevent duplicates
						terrainChunks[viewedCoord].UpdateChunk(viewerPos, maxViewDst);
						if ( terrainChunks[viewedCoord].IsVisible() ) {
							lastTerrainChunks.Add(terrainChunks[viewedCoord]);
						}
					} else {
						terrainChunks.Add(viewedCoord, new TerrainChunk(viewedCoord, chunkSize, transform, generator, mapMat, detailLevels));
					}
				}	
			}

		}

		private void HideChunks() {
			for ( int i = 0; i < lastTerrainChunks.Count; i++ ) {
				lastTerrainChunks[i].SetVisible(false);
			}
		}
	}

	[System.Serializable]
	public struct LODInfo {
		public int LOD;
		public float visibleThreshold;
	}
}
