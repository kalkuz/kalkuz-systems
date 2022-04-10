using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Attributes;
using UnityEditor;
using UnityEngine;

using HeaderAttribute = KalkuzSystems.Attributes.HeaderAttribute;

namespace KalkuzSystems.Customization.SkinSystem
{
    [CreateAssetMenu(fileName = "New Skin Data", menuName = "Kalkuz Systems/Customization/Skin Data")]
    public class SkinData : ScriptableObject
    {
        [SerializeField, ReadOnly] private uint skinID;
        [SerializeField, HideInInspector] private bool idSetBefore;

        [SerializeField] private string skinName;
        [SerializeField] private string skinDescription;

        [Header("Skin Assets"), SerializeField] private Sprite sprite;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private GameObject prefab;

        public uint SkinID => skinID;
        public bool IDSetBefore => idSetBefore;
        public string SkinName => skinName;
        public string SkinDescription => skinDescription;
        
        public Sprite Sprite => sprite;
        public Mesh Mesh => mesh;
        public Material Material => material;
        public GameObject Prefab => prefab;

        private void OnEnable()
        {
            if (idSetBefore) return;

            var guids = AssetDatabase.FindAssets("t:SkinData");
            var occupiedSkinIDs = new List<uint>();

            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<SkinData>(AssetDatabase.GUIDToAssetPath(guid));

                if (asset != this) occupiedSkinIDs.Add(asset.skinID);
            }

            for (uint i = 0; i < uint.MaxValue; i++)
            {
                if (!occupiedSkinIDs.Contains(i))
                {
                    Debug.Log($"First available id was {i}. Setting the skinID to {i}.");
                    skinID = i;
                    idSetBefore = true;
                    break;
                }
            }
        }
    }
}