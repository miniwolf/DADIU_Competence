using UnityEditor;
using AssemblyCSharp;
using UnityEngine;

namespace AssemblyCSharpEditor {
	[CustomEditor(typeof(MapGenerator))]
	public class MapGeneratorEditor : Editor {
		public override void OnInspectorGUI() {
			MapGenerator generator = (MapGenerator)target;

			if ( DrawDefaultInspector() ) {
				if ( generator.IsAutoUpdate() ) {
					//generator.DrawInEditor();
				}
			}

			if ( GUILayout.Button("Generate") ) {
				//generator.DrawInEditor();
			}
		}
	}
}
