using UnityEngine;

namespace Tanks
{
    public class ApplyTransformChangeToChildren : MonoBehaviour
    {
        private void Update()
        {
            var dp = transform.position - Vector3.zero;
            if (dp != Vector3.zero)
            {
                dp = transform.InverseTransformVector(dp);

                transform.localPosition = Vector3.zero;
                foreach (Transform child in transform)
                {
                    child.localPosition += dp;
                }
            }
        }
    }
}
