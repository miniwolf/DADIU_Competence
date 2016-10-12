using UnityEditor;
using AssemblyCSharp;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
