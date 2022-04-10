using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.Pooling
{
    public class Pool
    {
        protected Queue<PoolObject> queue;

        public Pool Initialize()
        {
            queue = new Queue<PoolObject>();
            return this;
        }

        public void AddToPool(PoolObject objectRef)
        {
            queue.Enqueue(objectRef);
        }

        public PoolObject Request(PoolObject objectRef)
        {
            if (queue.Count > 0) return queue.Dequeue();
            else
            {
                return GameObject.Instantiate(objectRef.gameObject).GetComponent<PoolObject>();
            }
        }
    }
}
