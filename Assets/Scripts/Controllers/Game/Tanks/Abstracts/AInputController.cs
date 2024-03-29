﻿using Common;
using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class AInputController : MonoBehaviourPun
    {
        [field: SerializeField]
        public MovementController MovementController { get; private set; }
        [field: SerializeField]
        public BulletController BulletController { get; private set; }

        private Vector2Int _direction = UVector2Int.Max;

        public Vector2Int Direction { get; set; }

        public bool Shooting { get; set; }

        public abstract bool IsPlayer { get; }

        public virtual bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        private void UpdateInput()
        {
            if (_direction != Direction)
            {
                OnMovementChange(Direction);
                _direction = Direction;
            }

            if (Direction != Vector2Int.zero)
            {
                MovementController.SetMovement(Direction);
            }
            else
            {
                MovementController.StopMovement();
            }

            if (Shooting)
            {
                if (BulletController.TryShoot(out var bullet))
                {
                    bullet.IsPlayer = IsPlayer;
                }
            }
        }

        protected virtual void OnMovementChange(Vector2Int value)
        {
        }

        #region Unity methods
        protected virtual void Update()
        {
            UpdateInput();
        }

        private void OnDisable()
        {
            MovementController.StopMovement();
        }
        #endregion
    }
}
