using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    public abstract class Soulbond : Buff
    {
        [HideInInspector] public List<GameObject> currentBonds;
        public bool timed;
        public int capacity;

        public abstract void Bond(params GameObject[] objects);
    }
}
