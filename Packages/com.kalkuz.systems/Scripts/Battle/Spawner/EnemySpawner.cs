using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<EnemySwarm> swarms;
        public bool spawnTillListEnds;
        public bool waitForCurrentSwarm;
        public float spawnRateBetweenSwarms, spawnRateInSwarmEntities;
        public float spawnRange;

        private void Start()
        {
            if (swarms != null)
            {
                StartCoroutine(Spawn());
            }
        }

        IEnumerator Spawn()
        {
            List<GameObject> spawned = new List<GameObject>();
            while (swarms.Count != 0)
            {
                if (waitForCurrentSwarm)
                {
                    while (spawned.Count > 0)
                    {
                        foreach (GameObject g in spawned.ToArray())
                        {
                            if (g.GetComponentInChildren<Animator>().GetBool("Died")) spawned.Remove(g);
                        }
                        yield return new WaitForSeconds(1);
                    }
                }

                yield return new WaitForSeconds(spawnRateBetweenSwarms);

                int randomIndex = Random.Range(0, swarms.Count);
                foreach (SwarmData swarmData in swarms[randomIndex].swarmDatas)
                {
                    for (int i = 0; i < swarmData.count; i++)
                    {
                        Vector3 randomOffset = new Vector3(Random.Range(-spawnRange, spawnRange), 0f, Random.Range(-spawnRange, spawnRange));
                        spawned.Add(Instantiate(swarmData.entity, transform.position + randomOffset, Quaternion.identity));
                        yield return new WaitForSeconds(spawnRateInSwarmEntities);
                    }
                }

                if (spawnTillListEnds) swarms.RemoveAt(randomIndex);
            }
        }
    }
}
