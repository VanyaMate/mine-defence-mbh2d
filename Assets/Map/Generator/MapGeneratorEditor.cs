#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Map.Generator
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (target == null) return;

            DrawDefaultInspector();

            MapGenerator generator = (MapGenerator)target;

            if (GUILayout.Button("Сгенерировать карту"))
            {
                generator.GenerateMap();
            }
        }
    }
}
#endif