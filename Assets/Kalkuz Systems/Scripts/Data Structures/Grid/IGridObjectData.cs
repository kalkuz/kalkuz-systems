using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    public interface IGridObjectData
    {
        Vector2Int CellWiseDimensions { get; set; }
    }
}

