using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Grid
{
    [System.Serializable]
    public class GridInteraction<T> where T : MonoBehaviour, IGridObject
    {
        public bool preview;
        public LayerMask raycastGridHitLayers;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Material previewMaterialApplicable, previewMaterialNonApplicable;

        private MeshFilter mf;

        public void OnValidate()
        {
            if (prefab == null || !prefab.TryGetComponent<T>(out T b))
            {
                Debug.LogError($"The object requires a prefab with {typeof(T)} script.");
                return;
            }
            else
            {
                mf = prefab.GetComponentInChildren<MeshFilter>();
            }
        }
        public void RotateObject(bool clockwise, GridAlignment alignment)
        {
            T obj = prefab.GetComponent<T>();

            Vector3 rotated;

            switch (alignment)
            {
                case GridAlignment.XY:
                    rotated = Quaternion.AngleAxis(180f, Vector2.one) * mf.transform.localPosition.CullAxes(Vector3Utilities.Vector3Axis.Z);
                    mf.transform.localPosition = rotated + mf.transform.localPosition.z * Vector3.forward;

                    mf.transform.Rotate(clockwise ? 90 * Vector3.forward : 90 * Vector3.back);
                    break;
                case GridAlignment.XZ:
                    rotated = Quaternion.AngleAxis(180f, Vector2.one.Vector2ToVector3(0).XY2XZ(0)) * mf.transform.localPosition.CullAxes(Vector3Utilities.Vector3Axis.Y);
                    mf.transform.localPosition = rotated + mf.transform.localPosition.y * Vector3.up;

                    mf.transform.Rotate(clockwise ? 90 * Vector3.up : 90 * Vector3.down);
                    break;
                case GridAlignment.YZ:
                    rotated = Quaternion.AngleAxis(180f, Vector2.one.Vector2ToVector3(0).XY2YZ(0)) * mf.transform.localPosition.CullAxes(Vector3Utilities.Vector3Axis.X);
                    mf.transform.localPosition = rotated + mf.transform.localPosition.x * Vector3.right;

                    mf.transform.Rotate(clockwise ? 90 * Vector3.right : 90 * Vector3.left);
                    break;
                default:
                    break;
            }

            obj.Data.CellWiseDimensions = new Vector2Int(obj.Data.CellWiseDimensions.y, obj.Data.CellWiseDimensions.x);
        }
        public T GetObject()
        {
            return prefab.GetComponent<T>();
        }
        public void PreviewObject(Vector3 position, bool isApplicable)
        {
            Debug.DrawLine(position, mf.transform.localPosition, Color.green);
            PreviewObject(position, isApplicable, Vector3.one);
        }
        public void PreviewObject(Vector3 position, bool isApplicable, Vector3 scaleFactor)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(position + Vector3.Scale(mf.transform.position, scaleFactor), mf.transform.rotation, Vector3.Scale(mf.transform.localScale, scaleFactor));
            Graphics.DrawMesh(mf.sharedMesh, matrix, isApplicable ? previewMaterialApplicable : previewMaterialNonApplicable, 0);
        }
        public GameObject InstantiateObject(Vector3 position)
        {
            return GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
        public GameObject InstantiateObject(Vector3 position, Vector3 scale)
        {
            GameObject go = GameObject.Instantiate(prefab, position, Quaternion.identity);
            go.transform.localScale = Vector3.Scale(go.transform.localScale, scale);
            return go;
        }
    }
}
