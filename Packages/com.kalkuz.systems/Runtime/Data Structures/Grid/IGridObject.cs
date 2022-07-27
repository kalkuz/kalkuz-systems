using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    public interface IGridObject
    {
        IGridObjectData Data { get; }
    }
}
