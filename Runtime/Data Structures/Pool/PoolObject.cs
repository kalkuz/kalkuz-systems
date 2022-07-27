using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Pooling
{
    public class PoolObject : MonoBehaviour
    {
        [SerializeField] private string id;
        public string ID => id;

        public void ReturnToPool()
        {
            UniversalPoolProvider.GetPool(id).AddToPool(this);
        }
    }
}
