using Common.Injection;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Tanks.Extensions;
using Tanks.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks
{
    [RequireComponent(typeof(PhotonView))]
    public class GameController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public GameData GameProperties { get; private set; }
        [field: SerializeField]
        public ScenesData GameScenes { get; private set; }
        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }
        [field: DI_Inject]
        public GameUIController GameUIController { get; private set; }
        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        private bool _finished = false;

        private bool IsStatueDestroyed(ETeam team)
        {
            var statue = StatuesController.Statues[team];
            return statue.IsDestroyed;
        }

        private bool TryGetIsStatueDestroyed(out ETeam team)
        {
            team = ETeam.None;
            if (IsStatueDestroyed(ETeam.A))
                team = ETeam.A;
            else if (IsStatueDestroyed(ETeam.B))
                team = ETeam.B;
            return team != ETeam.None;
        }

        private void CheckStatuesDestroyed()
        {
            var room = PhotonNetwork.CurrentRoom;

            if (!_finished &&
                TryGetIsStatueDestroyed(out var team))
            {
                room.IncrTeamWins(team.Flip());

                EndGame();
                RPCEndGame();

                _finished = true;
            }
        }

        private IEnumerator FinishGame(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            OnGameFinished();
        }

        private void OnGameFinished()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(GameScenes.GameScene);
            }
            else
            {
                SceneManager.LoadScene(GameScenes.GameScene);
            }
        }

        public void EndGame(float lag = 0.0f)
        {
            GameUIController.OnGameEnded();
            TanksController.GetMineTank().InputController.IsEnabled = false;

            StartCoroutine(FinishGame(GameProperties.endGameDelay - lag));
        }

        public void RPCEndGame()
        {
            photonView.RPC(nameof(RPCEndGame_Internal), RpcTarget.Others);
        }

        [PunRPC]
        private void RPCEndGame_Internal(PhotonMessageInfo info)
        {
            EndGame(info.GetLag());
        }

        #region Photon methods
        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();

            PhotonNetwork.LocalPlayer.ResetProperties();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene(GameScenes.LobbyScene);
        }
        #endregion

        #region Unity methods
        private void Awake()
        {
            DI_Binder.Bind(this);
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CheckStatuesDestroyed();
            }
        }
        #endregion
    }
}
