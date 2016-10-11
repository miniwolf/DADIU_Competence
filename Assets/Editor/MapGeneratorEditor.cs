using UnityEditor;
using AssemblyCSharp;
using UnityEngine;

namespace AssemblyCSharpEditor {
	[CustomEditor(typeof(MapGenerator))]
	public class MapGeneratorEditor : Editor {
		public override void OnInspectorGUI() {
			var generator = (MapGenerator)target;

			if ( DrawDefaultInspector() ) {
				if ( generator.IsAutoUpdate() ) {
					generator.Generate();
				}
			}

			if ( GUILayout.Button("Generate") ) {
				generator.Generate();
			}
		}
	}
}
