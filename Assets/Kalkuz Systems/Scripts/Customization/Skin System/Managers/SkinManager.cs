using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem
{
    /// <summary>
    /// Manages the replacement of skins of an object.
    /// </summary>
    public abstract class SkinManager : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="SkinContainer"/> asset that contains the list of several <see cref="SkinData"/> assets
        /// </summary>
        [SerializeField] protected SkinContainer skinContainer;

        /// <summary>
        /// Changes the related skin component of the <see cref="Renderer"/> to the requested one
        /// </summary>
        /// <param name="skinIndex">The index of skin in the list of skins in cluster</param>
        /// <returns>Can the change be done, or is successful</returns>
        public virtual bool Change(int skinIndex)
        {
            if (skinContainer.Skins.Count <= skinIndex)
            {
                Debug.LogError($"Skin Index {skinIndex} is more than the count of skins in cluster.");
                return false;
            }

            if (skinContainer == null)
            {
                Debug.LogWarning($"No skin cluster provided to {gameObject.name}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Automatically called from <see cref="Change"/> method. Control for the user actually purchased the skin.
        /// </summary>
        /// <returns>Whether the skin replacement is valid or not.</returns>
        protected abstract bool Validate(string skinID);
    }
}
