using Common.Extensions;
using UnityEngine;
using Random = System.Random;

namespace Tanks
{
    public class SpawnPlayer : MonoBehaviour
    {
        private static readonly Random kRandom = new Random();

        public string playerPrefabPath;
        public Transform[] spawnPoints;

        private void Start()
        {
            var spawnPoint = kRandom.NextItem(spawnPoints);

            Network.Instantiate(playerPrefabPath, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
