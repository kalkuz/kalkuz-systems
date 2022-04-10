using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem
{
    /// <summary>
    /// Container for skin alternatives that is used to manage single object's skin.
    /// </summary>
    [CreateAssetMenu(fileName = "New Skin Container", menuName = "Kalkuz Systems/Customization/Skin Container")]
    public class SkinContainer : ScriptableObject
    {
        /// <summary>
        /// List of <see cref="SkinData"/> assets.
        /// </summary>
        [SerializeField] private List<SkinData> skins;

        /// <summary>
        /// Read only list of <see cref="SkinData"/> assets.
        /// </summary>
        public IReadOnlyList<SkinData> Skins => skins;
    }
}