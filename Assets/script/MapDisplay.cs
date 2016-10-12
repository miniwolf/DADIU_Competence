using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class MapDisplay : MonoBehaviour {
		public Renderer textureRenderer;
		public MeshFilter filter;
		public MeshRenderer meshRenderer;

		public void DrawTexture(Texture2D tex) {
			textureRenderer.sharedMaterial.mainTexture = tex;
			textureRenderer.transform.localScale = new Vector3(tex.width, 1, tex.height);
		}

		public void DrawMesh(MeshData data, Texture2D texture) {
			filter.sharedMesh = data.CreateMesh();
			meshRenderer.sharedMaterial.mainTexture = texture;
		}
	}
}
