using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem
{
    /// <summary>
    /// Used when it is needed to change the <see cref="Sprite"/> and <see cref="Material"/> of an object. 
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSkinManager : SkinManager
    {
        /// <summary>
        /// <see cref="SpriteRenderer"/> component of target
        /// </summary>
        [Header("References"), SerializeField] private SpriteRenderer spriteRenderer;

        public override bool Change(int skinIndex)
        {
            if (!base.Change(skinIndex)) return false;

            if (spriteRenderer == null)
            {
                Debug.LogWarning($"No sprite renderer provided to {gameObject.name}");
                return false;
            }

            var skinData = skinContainer.Skins[skinIndex];
            if (!Validate(skinData.SkinID))
            {
                Debug.LogWarning($"Skin validation failed.");
                return false;
            }

            if (skinData.Sprite) spriteRenderer.sprite = skinData.Sprite;
            if (skinData.Material) spriteRenderer.material = skinData.Material;

            return true;
        }

        protected override bool Validate(uint skinID)
        {
            // Todo: implement functionality
            return true;
        }
    }
}