using Common.Injection;
using ExitGames.Client.Photon;
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
        public GameProperties GameProperties { get; private set; }
        [field: DI_Inject]
        public StatuesController StatuesController { get; private set; }
        [field: DI_Inject]
        public GameUIController GameUIController { get; private set; }
        [field: DI_Inject]
        public TanksController TanksController { get; private set; }

        private bool _finished = false;

        private bool IsStatueDestroyed(ETeam team)
        {
            var statue = StatuesController.GetStatue(team);
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
            yield return new WaitForRealSeconds(delay);
            OnGameFinished();
        }

        private void OnGameFinished()
        {
            const string GAME_SCENE = "Game";

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(GAME_SCENE);
            }
            else
            {
                SceneManager.LoadScene(GAME_SCENE);
            }
        }

        public void EndGame(float lag = 0.0f)
        {
            GameUIController.OnGameEnded();
            TanksController.GetMineTank().IsEnabled = false;

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
            SceneManager.LoadScene("Lobby");
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

        private void OnDestroy()
        {
            DI_Binder.Unbind(this);
        }
        #endregion
    }
}
