using Photon.Pun;
using System.Collections.Generic;
using Tanks.Extensions;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class FortifyController : MonoBehaviourPun
    {
        public float duration = 1.0f;
        
        [field: SerializeField]
        public Stone StonePrefab { get; private set; }
        [field: SerializeField]
        public Brick BrickPrefab { get; private set; }
        [field: SerializeField]
        public GameObject[] Stones { get; private set; }

        private List<Brick> _bricks = new List<Brick>();
        private float _switchtime;

        public void Fortify()
        {
            Fortify(duration);
        }

        public void Fortify(float duration)
        {
            var time = Time.time;
            _switchtime = time + duration;

            foreach (var brick in _bricks)
            {
                if (brick != null)
                {
                    Destroy(brick.gameObject);
                }
            }
            _bricks.Clear();

            foreach (var stone in Stones)
            {
                stone.SetActive(true);
            }
        }

        public void RPCFortify()
        {
            RPCFortify(duration);
        }

        public void RPCFortify(float duration)
        {
            photonView.RPC(nameof(RPCFortify_Internal), RpcTarget.Others, duration);
        }

        [PunRPC]
        private void RPCFortify_Internal(float duration, PhotonMessageInfo info)
        {
            Fortify(duration - info.GetLag());
        }

        public void SwitchToBricks()
        {
            foreach (var stone in Stones)
            {
                stone.SetActive(false);

                var brick = Instantiate(BrickPrefab, stone.transform.position, stone.transform.rotation, transform);
                _bricks.Add(brick);
            }
        }

        private void CheckSwitch()
        {
            var time = Time.time;
            if (_switchtime < time)
            {
                _switchtime = float.MaxValue;

                SwitchToBricks();
            }
        }

        #region Unity methods
        private void Update()
        {
            CheckSwitch();
        }
        #endregion
    }
}
