using Common.Mathematics;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Tanks
{
    public class SpawnPlayer : MonoBehaviour
    {
        private static readonly Random kRandom = new Random();

        public string playerPrefabPath;
        public string friendPrefabPath;
        public string enemyPrefabPath;
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

            PhotonNetwork.Instantiate(playerPrefabPath, spawn.transform.position, spawn.transform.rotation);
        }

        private void Start()
        {
            StartCoroutine(FindAndSpawn());
        }
    }
}
