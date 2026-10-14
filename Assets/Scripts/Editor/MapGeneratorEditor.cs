using UnityEngine;
using System;
using UnityEditor;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator lMap = (MapGenerator)target;
            if (DrawDefaultInspector()) lMap.GenerateMap();
            if (GUILayout.Button("Generate")) lMap.GenerateMap();
        }
    }
}