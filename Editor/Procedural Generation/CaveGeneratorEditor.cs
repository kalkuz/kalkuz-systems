using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.ProceduralGeneration
{

    [CustomEditor(typeof(CaveGenerator))]
    public class CaveGeneratorEditor : GeneratorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
}
