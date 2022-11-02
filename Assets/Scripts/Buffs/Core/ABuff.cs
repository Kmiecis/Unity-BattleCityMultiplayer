using UnityEngine;

namespace Tanks
{
    public abstract class ABuff : MonoBehaviour
    {
        public EBuffType buffType;

        protected ETeam _team;
        protected float _lag;

        public void Setup(ETeam team, float lag)
        {
            _team = team;
            _lag = lag;
        }
    }
}
