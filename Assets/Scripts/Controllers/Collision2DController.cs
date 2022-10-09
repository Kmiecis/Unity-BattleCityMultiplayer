using System;
using UnityEngine;

namespace Tanks
{
    public class Collision2DController : MonoBehaviour
    {
        public event Action<Collision2D> CalledOnEnter;
        public event Action<Collision2D> CalledOnStay;
        public event Action<Collision2D> CalledOnExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CalledOnEnter?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CalledOnStay?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CalledOnExit?.Invoke(collision);
        }
    }
}
