using UnityEngine;

public static class TextureGenerator {
	public static Texture2D TextureFromColourMap(Color[] map, int width, int height) {
		Texture2D tex = new Texture2D(width, height);
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Repeat;
		tex.SetPixels(map);
		tex.Apply();

		return tex;
	}

	public static Texture2D TextureFromHeighMap(float[,] map) {
		int width = map.GetLength(0);
		int height = map.GetLength(1);

		var colourMap = SetupColourmap(map, width, height);

		return TextureFromColourMap(colourMap, width, height);
	}

	private static Color[] SetupColourmap(float[,] map, int width, int height) {
		var colourMap = new Color[width * height];
		for ( int x = 0; x < width; x++ ) {
			for ( int y = 0; y < height; y++ ) {
				colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, map[x, y]);
			}
		}
		return colourMap;
	}
}
