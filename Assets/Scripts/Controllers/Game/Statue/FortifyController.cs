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

            for (int i = 0; i < _bricks.Count; ++i)
            {
                var brick = _bricks[i];
                if (brick != null)
                {
                    if (brick.IsFractured)
                    {
                        Destroy(brick.gameObject);
                        _bricks[i] = null;
                    }
                    else
                    {
                        brick.gameObject.SetActive(false);
                    }
                }
            }

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
            for (int i = 0; i < Stones.Length; ++i)
            {
                var stone = Stones[i];
                stone.SetActive(false);

                var brick = _bricks[i];
                if (brick == null)
                {
                    _bricks[i] = Instantiate(BrickPrefab, stone.transform.position, stone.transform.rotation, transform);
                }
                else
                {
                    brick.gameObject.SetActive(true);
                }
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
        private void Start()
        {
            for (int i = 0; i < Stones.Length; ++i)
            {
                _bricks.Add(null);
            }
        }

        private void Update()
        {
            CheckSwitch();
        }
        #endregion
    }
}
