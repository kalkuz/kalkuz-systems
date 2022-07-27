using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Attributes;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem
{
    [CreateAssetMenu(fileName = "New Skin Data", menuName = "Kalkuz Systems/Customization/Skin Data")]
    public class SkinData : ScriptableObject
    {
        [SerializeField] private string skinID;
        [SerializeField, HideInInspector] private bool idSetBefore;

        [SerializeField] private string skinName;
        [SerializeField] private string skinDescription;

        [Title("Skin Assets"), SerializeField] private Sprite sprite;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private GameObject prefab;

        public string SkinID => skinID;
        public string SkinName => skinName;
        public string SkinDescription => skinDescription;
        
        public Sprite Sprite => sprite;
        public Mesh Mesh => mesh;
        public Material Material => material;
        public GameObject Prefab => prefab;
    }
}