using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Attributes;
using KalkuzSystems.Utility.Arrays;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    public enum GridAlignment { XY, XZ, YZ }

    [System.Serializable]
    public class GridGeneric<T> where T : MonoBehaviour, IGridObject
    {
        [SerializeField] private GridAlignment alignment;

        [Min(1)]
        [SerializeField] private Vector2Int dimensions;
        [SerializeField] private Vector2 gridCell;
        private T[,] grid;

        [Title("Debug")]
        [SerializeField] Vector2Int indices;
        [SerializeField] [ReadOnly] T objectAtIndex;
        
        public void Initialize()
        {
            grid = new T[dimensions.x, dimensions.y];
        }

        public void Set(int x, int y, T obj)
        {
            if (!grid.ContainsIndices(x, y)) return;

            grid[x, y] = obj;
        }

        public bool TryPlace(int x, int y, T obj)
        {
            if (!CanBePlaced(x, y, obj)) return false;

            for (int i = x; i < x + obj.Data.CellWiseDimensions.x; i++)
            {
                for (int j = y; j < y + obj.Data.CellWiseDimensions.y; j++)
                {
                    Set(i, j, obj);
                }
            }

            return true;
        }
        public void Place(int x, int y, T obj)
        {
            for (int i = x; i < x + obj.Data.CellWiseDimensions.x; i++)
            {
                for (int j = y; j < y + obj.Data.CellWiseDimensions.y; j++)
                {
                    Set(i, j, obj);
                }
            }
        }

        public bool ContainsIndices(int x, int y)
        {
            return grid.ContainsIndices(x, y);
        }

        public GridAlignment GetAlignment() => alignment;

        public bool CanBePlaced(int x, int y, T obj)
        {
            int spanX = x + obj.Data.CellWiseDimensions.x - 1;
            int spanY = y + obj.Data.CellWiseDimensions.y - 1;
            if (!grid.ContainsIndices(x, y) || !grid.ContainsIndices(spanX, spanY))
            {
                return false;
            }

            for (int i = x; i <= spanX; i++)
            {
                for (int j = y; j <= spanY; j++)
                {
                    if (!IsAvailable(i, j)) return false;
                }
            }

            return true;
        }

        public T Get(int x, int y)
        {
            if (!grid.ContainsIndices(x, y)) throw new System.IndexOutOfRangeException();

            return grid[x, y];
        }

        public bool IsAvailable(int x, int y)
        {
            return grid[x, y] == null;
        }

        public void OnValidate()
        {
            if (grid == null) return;
            if (grid.ContainsIndices(indices.x, indices.y))
            {
                objectAtIndex = grid[indices.x, indices.y];
                Debug.Log(objectAtIndex);
            }
        }

        public Vector3 ScaleCorrection(Vector3 objectCells)
        {
            switch (alignment)
            {
                case GridAlignment.XY:
                    return Vector3.Scale(gridCell.Vector2ToVector3(1), objectCells);
                case GridAlignment.XZ:
                    return Vector3.Scale(gridCell.Vector2ToVector3(1), objectCells).XY2XZ();
                case GridAlignment.YZ:
                    return Vector3.Scale(gridCell.Vector2ToVector3(1), objectCells).XY2YZ();
                default:
                    return Vector3.Scale(gridCell.Vector2ToVector3(1), objectCells);
            }
        }

        public bool WorldPointToGridIndices(Vector3 worldOrigin, Vector3 point, out int x, out int y)
        {
            switch (alignment)
            {
                case GridAlignment.XY:
                    x = Mathf.FloorToInt((point.x - worldOrigin.x) / gridCell.x);
                    y = Mathf.FloorToInt((point.y - worldOrigin.y) / gridCell.y);
                    return true;
                case GridAlignment.XZ:
                    x = Mathf.FloorToInt((point.x - worldOrigin.x) / gridCell.x);
                    y = Mathf.FloorToInt((point.z - worldOrigin.z) / gridCell.y);
                    return true;
                case GridAlignment.YZ:
                    x = Mathf.FloorToInt((point.y - worldOrigin.y) / gridCell.x);
                    y = Mathf.FloorToInt((point.z - worldOrigin.z) / gridCell.y);
                    return true;
                default:
                    x = -1;
                    y = -1;
                    return false;
            }
        }
        public Vector3 GridIndicesToWorldPoint(Vector3 worldOrigin, int x, int y)
        {
            if (!grid.ContainsIndices(x, y)) throw new System.IndexOutOfRangeException();

            switch (alignment)
            {
                case GridAlignment.XY:
                    return new Vector3(x * gridCell.x, y * gridCell.y) + worldOrigin;
                case GridAlignment.XZ:
                    return new Vector3(x * gridCell.x, 0f, y * gridCell.y) + worldOrigin;
                case GridAlignment.YZ:
                    return new Vector3(0f, x * gridCell.x, y * gridCell.y) + worldOrigin;
                default:
                    return Vector3.zero + worldOrigin;
            }
        }

        #region "Grid Gizmos"
        public void OnDrawGizmos(Vector3 pivot)
        {
            if (grid == null) return;

            Gizmos.color = Color.red;

            switch (alignment)
            {
                case GridAlignment.XY:
                    DrawGridXY(pivot);
                    break;
                case GridAlignment.XZ:
                    DrawGridXZ(pivot);
                    break;
                case GridAlignment.YZ:
                    DrawGridYZ(pivot);
                    break;
                default:
                    break;
            }
        }
        void DrawGridXZ(Vector3 pivot)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (j == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, 0, j * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, 0, j * gridCell.y));
                    }
                    if (i == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, 0, j * gridCell.y), pivot + new Vector3(i * gridCell.x, 0, (j + 1) * gridCell.y));
                    }
                    Gizmos.DrawLine(pivot + new Vector3((i + 1) * gridCell.x, 0, j * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, 0, (j + 1) * gridCell.y));
                    Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, 0, (j + 1) * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, 0, (j + 1) * gridCell.y));
                }
            }
        }

        void DrawGridXY(Vector3 pivot)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (j == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, j * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, j * gridCell.y));
                    }
                    if (i == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, j * gridCell.y), pivot + new Vector3(i * gridCell.x, (j + 1) * gridCell.y));
                    }
                    Gizmos.DrawLine(pivot + new Vector3((i + 1) * gridCell.x, j * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, (j + 1) * gridCell.y));
                    Gizmos.DrawLine(pivot + new Vector3(i * gridCell.x, (j + 1) * gridCell.y), pivot + new Vector3((i + 1) * gridCell.x, (j + 1) * gridCell.y));
                }
            }
        }
        void DrawGridYZ(Vector3 pivot)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (j == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(0, i * gridCell.x, j * gridCell.y), pivot + new Vector3(0, (i + 1) * gridCell.x, j * gridCell.y));
                    }
                    if (i == 0)
                    {
                        Gizmos.DrawLine(pivot + new Vector3(0, i * gridCell.x, j * gridCell.y), pivot + new Vector3(0, i * gridCell.x, (j + 1) * gridCell.y));
                    }
                    Gizmos.DrawLine(pivot + new Vector3(0, (i + 1) * gridCell.x, j * gridCell.y), pivot + new Vector3(0, (i + 1) * gridCell.x, (j + 1) * gridCell.y));
                    Gizmos.DrawLine(pivot + new Vector3(0, i * gridCell.x, (j + 1) * gridCell.y), pivot + new Vector3(0, (i + 1) * gridCell.x, (j + 1) * gridCell.y));
                }
            }
        }

        #endregion
    }
}
