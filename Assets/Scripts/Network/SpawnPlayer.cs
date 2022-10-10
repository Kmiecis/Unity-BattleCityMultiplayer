using Common.Mathematics;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Tanks
{
    public class SpawnPlayer : MonoBehaviour
    {
        private static readonly Random kRandom = new Random();

        public string playerPrefabPath;
        public Spawn[] spawns;

        private bool TryFindSpawn(out Spawn spawn)
        {
            var spawnIndex = kRandom.Next(spawns.Length);
            for (int i = spawnIndex; i != spawnIndex - 1; i = Mathx.NextIndex(i, spawns.Length))
            {
                spawn = spawns[i];
                if (spawn.IsValid)
                {
                    return true;
                }
            }

            spawn = null;
            return false;
        }

        private IEnumerator FindAndSpawn()
        {
            Spawn spawn;
            while (!TryFindSpawn(out spawn))
            {
                yield return new WaitForSecondsRealtime(1.0f);
            }

            Network.Instantiate(playerPrefabPath, spawn.transform.position, spawn.transform.rotation);
        }

        private void Start()
        {
            StartCoroutine(FindAndSpawn());
        }
    }
}
