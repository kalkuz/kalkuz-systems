using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.ProceduralGeneration
{
    [CustomEditor(typeof(TopDownMapGenerator))]
    public class TopDownMapGeneratorEditor : GeneratorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
