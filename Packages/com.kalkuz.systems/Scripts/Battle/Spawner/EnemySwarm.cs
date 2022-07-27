using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle
{
    [System.Serializable]
    public class EnemySwarm
    {
        public List<SwarmData> swarmDatas;
    }

    [System.Serializable]
    public class SwarmData
    {
        public GameObject entity;
        public int count;
    }
}
