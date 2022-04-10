using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Pooling
{
    public static class UniversalPoolProvider
    {
        private static Dictionary<string, Pool> m_pools;

        public static Pool GetPool(string id)
        {
            m_pools ??= new Dictionary<string, Pool>();
            
            if (m_pools.TryGetValue(id, out Pool value))
            {
                return value;
            }
            else
            {
                m_pools[id] = new Pool().Initialize();
                return m_pools[id];
            }
        }
    }
}
