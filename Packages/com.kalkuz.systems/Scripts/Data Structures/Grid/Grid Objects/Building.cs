using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    public class Building : MonoBehaviour, IGridObject
    {
        [SerializeField] private BuildingData buildingData;

        public IGridObjectData Data => buildingData;
    }
}
