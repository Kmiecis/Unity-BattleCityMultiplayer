using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class Network
    {
        public static GameObject Instantiate(string prefabPath)
        {
            return Instantiate(prefabPath, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Instantiate(string prefabPath, Vector3 position, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(prefabPath, position, rotation);
        }

        public static void Destroy(GameObject gameObject)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
