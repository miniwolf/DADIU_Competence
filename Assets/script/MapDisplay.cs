using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class MapDisplay : MonoBehaviour {
		public Renderer textureRenderer;

		public void DrawTexture(Texture2D tex) {
			textureRenderer.sharedMaterial.mainTexture = tex;
			textureRenderer.transform.localScale = new Vector3(tex.width, 1, tex.height);
		}
	}
}
