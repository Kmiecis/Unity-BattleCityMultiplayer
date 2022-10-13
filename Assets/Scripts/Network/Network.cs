using Photon.Pun;
using System;
using UnityEngine;

namespace Tanks
{
    public class Network
    {
        private static bool HasNetwork
        {
            get => PhotonNetwork.CurrentRoom != null;
        }

        public static GameObject Instantiate(string prefabPath)
        {
            return Instantiate(prefabPath, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Instantiate(string prefabPath, Vector3 position, Quaternion rotation)
        {
            return HasNetwork ?
                NetworkInstantiate(prefabPath, position, rotation) :
                LocalInstantiate(prefabPath, position, rotation);
        }

        private static GameObject NetworkInstantiate(string prefabPath, Vector3 position, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(prefabPath, position, rotation);
        }

        private static GameObject LocalInstantiate(string prefabPath, Vector3 position, Quaternion rotation)
        {
            var prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab, position, rotation);
            }
            return null;
        }

        public static void Destroy(GameObject gameObject)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public static void RPC(PhotonView view, RpcTarget target, string methodName, params object[] parameters)
        {
            if (HasNetwork)
            {
                view.RPC(methodName, target, parameters);
            }
            else
            {
                view.SendMessage(methodName, parameters);
            }
        }
    }
}
