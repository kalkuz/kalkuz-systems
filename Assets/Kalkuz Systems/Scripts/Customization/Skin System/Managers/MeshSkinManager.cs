using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem
{
    /// <summary>
    /// Used when it is needed to change the <see cref="Mesh"/> and <see cref="Material"/> of an object. 
    /// </summary>
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class MeshSkinManager : SkinManager
    {
        /// <summary>
        /// <see cref="MeshFilter"/> component of target
        /// </summary>
        [Header("References"), SerializeField] private MeshFilter meshFilter;

        /// <summary>
        /// <see cref="MeshRenderer"/> component of target
        /// </summary>
        [SerializeField] private MeshRenderer meshRenderer;

        public override bool Change(int skinIndex)
        {
            if (!base.Change(skinIndex)) return false;

            if (meshRenderer == null)
            {
                Debug.LogWarning($"No mesh renderer provided to {gameObject.name}");
                return false;
            }

            if (meshFilter == null)
            {
                Debug.LogWarning($"No mesh filter provided to {gameObject.name}");
                return false;
            }

            var skinData = skinContainer.Skins[skinIndex];
            if (!Validate(skinData.SkinID))
            {
                Debug.LogWarning($"Skin validation failed.");
                return false;
            }

            if (skinData.Material) meshRenderer.material = skinData.Material;
            if (skinData.Mesh) meshFilter.mesh = skinData.Mesh;

            return true;
        }

        protected override bool Validate(uint skinID)
        {
            // Todo: implement functionality
            return true;
        }
    }
}
