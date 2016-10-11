using System;
using UnityEngine;

namespace AssemblyCSharp {
	public class MapDisplay : MonoBehaviour {
		public Renderer textureRenderer;

		public void DrawMap(float[,] map) {
			int width = map.GetLength(0);
			int height = map.GetLength(1);

			var texture = new Texture2D(width, height);

			var colourMap = SetupColourmap(map, width, height);

			texture.SetPixels(colourMap);
			texture.Apply();

			textureRenderer.sharedMaterial.mainTexture = texture;
			textureRenderer.transform.localScale = new Vector3(width, 1, height);
		}

		private Color[] SetupColourmap(float[,] map, int width, int height) {
			var colourMap = new Color[width * height];
			for ( int x = 0; x < width; x++ ) {
				for ( int y = 0; y < height; y++ ) {
					colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, map[x, y]);
				}
			}
			return colourMap;
		}
	}
}
