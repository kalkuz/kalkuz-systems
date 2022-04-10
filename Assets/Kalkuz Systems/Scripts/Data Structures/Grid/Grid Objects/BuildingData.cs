using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Grid Object/Building", fileName = "New Building")]
    public class BuildingData : ScriptableObject, IGridObjectData
    {
        [SerializeField] private Vector2Int cellWiseDimensions;

        public Vector2Int CellWiseDimensions { get => cellWiseDimensions; set => cellWiseDimensions = value; }
    }
}
