using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.ProceduralGeneration
{
    [CustomEditor(typeof(SideViewMapGenerator))]
    public class SideViewMapGeneratorEditor : GeneratorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
