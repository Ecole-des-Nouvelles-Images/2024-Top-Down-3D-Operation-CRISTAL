using Game.Terrain.Procedural;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapGenerator))] [CanEditMultipleObjects]
    public class MapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGen = (MapGenerator)target;

            if (DrawDefaultInspector()) {
                if (mapGen.AutoUpdate) {
                    mapGen.GenerateMap();
                }
            }

            if (GUILayout.Button("Generate")) {
                mapGen.GenerateMap();
            }
        }
    }
}
