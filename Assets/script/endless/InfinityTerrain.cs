using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class InfinityTerrain : MonoBehaviour {
		const float moveThreshold = 25f;
		const float sqrMoveThreshold = moveThreshold * moveThreshold;

		public LODInfo[] detailLevels;
		public static float maxViewDst;

		public Material mapMat;

		private int chunkSize;
		private int chunksVisible;

		private Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
		private readonly List<TerrainChunk> lastTerrainChunks = new List<TerrainChunk>();

		private Transform viewer;
		private MapGenerator generator;
		private Vector2 viewerPos = Vector2.zero;
		private Vector2 oldViewerPos;

		void Start() {
			viewer = GameObject.FindGameObjectWithTag(TagConstants.PLAYER).transform;
			generator = GameObject.FindGameObjectWithTag(TagConstants.MAPGENERATOR).GetComponent<MapGenerator>();
			maxViewDst = detailLevels[detailLevels.Length - 1].visibleThreshold;
			chunkSize = MapGenerator.chunkSize - 1;
			chunksVisible = Mathf.RoundToInt(maxViewDst / chunkSize);

			UpdateChunks();
		}

		/// <summary>
		/// From viewer position we update the chunks if we have moved a distanced based on our threshold
		/// </summary>
		void Update() {
			viewerPos = new Vector2(viewer.position.x, viewer.position.z);

			if ( (oldViewerPos - viewerPos).sqrMagnitude > sqrMoveThreshold ) {
				oldViewerPos = viewerPos;
				UpdateChunks();
			}
		}

		/// <summary>
		/// Update chunks within viewable range
		/// </summary>
		void UpdateChunks() {
			HideChunks();
			lastTerrainChunks.Clear();

			int currentX = Mathf.RoundToInt(viewerPos.x / chunkSize);
			int currentY = Mathf.RoundToInt(viewerPos.y / chunkSize);

			for ( int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++ ) {
				for ( int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++ ) {
					var viewedCoord = new Vector2(currentX + xOffset, currentY + yOffset);
					AddOrUpdateChunk(viewedCoord);
				}	
			}
		}

		private void AddOrUpdateChunk(Vector2 viewedCoord) {
			if ( terrainChunks.ContainsKey(viewedCoord) ) { // Prevent duplicates
				terrainChunks[viewedCoord].UpdateChunk(viewerPos, maxViewDst, lastTerrainChunks);
			} else {
				terrainChunks.Add(viewedCoord, new TerrainChunk(viewedCoord, chunkSize, transform, generator, mapMat, detailLevels, UpdateChunks));
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
