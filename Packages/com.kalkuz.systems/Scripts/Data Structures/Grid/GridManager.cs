using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    public class GridManager : MonoBehaviour
    {
        //Declare the type you will be using
        public GridGeneric<Building> grid;
        public GridInteraction<Building> gridInteraction;

        private void Awake()
        {
            grid.Initialize(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) gridInteraction.preview = !gridInteraction.preview;
            if (gridInteraction.preview && Input.GetKeyDown(KeyCode.R)) gridInteraction.RotateObject(false, grid.GetAlignment());

            if (gridInteraction.preview)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, gridInteraction.raycastGridHitLayers))
                {
                    if (grid.WorldPointToGridIndices(hit.point, out int x, out int y))
                    {
                        //Vector3 scaleCorrection = grid.ScaleCorrection(((Vector2)gridInteraction.GetObject().Data.CellWiseDimensions).Vector2ToVector3(1));
                        bool canPlace = grid.CanBePlaced(x, y, gridInteraction.GetObject());

                        if (grid.ContainsIndices(x, y)) gridInteraction.PreviewObject(grid.GridIndicesToWorldPoint(x, y), canPlace);

                        if (canPlace && Input.GetMouseButtonDown(0))
                        {
                            grid.Place(x, y, gridInteraction.GetObject());
                            gridInteraction.InstantiateObject(grid.GridIndicesToWorldPoint(x, y));
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000, gridInteraction.raycastGridHitLayers))
                {
                    if (grid.WorldPointToGridIndices(hit.point, out int x, out int y))
                    {
                        Debug.Log(grid.Get(x, y));
                    }
                }
            }
        }

        private void OnValidate()
        {
            gridInteraction.OnValidate();
            grid.OnValidate();
        }

        private void OnDrawGizmos()
        {
            grid.OnDrawGizmos(transform.position);
        }
    }
}
