using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.ProceduralGeneration
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : Editor
    {
        public bool autoUpdate;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Generator generator = target as Generator;
            autoUpdate = EditorGUILayout.Toggle("Auto Update", autoUpdate);
            EditorGUILayout.Space();

            if (DrawDefaultInspector())
            {
                if (autoUpdate)
                {
                    generator.Generate();
                }
            }

            if (GUILayout.Button("Randomize Seed"))
            {
                foreach (Wave w in generator.waves)
                {
                    w.RandomizeSeed();
                }
                generator.Generate();
            }

            if (GUILayout.Button("Generate"))
            {
                generator.Generate();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
