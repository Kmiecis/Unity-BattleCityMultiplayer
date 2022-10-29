﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Tanks.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Tanks
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public GameProperties GameProperties { get; private set; }
        [field: SerializeField]
        public StatuesController StatuesController { get; private set; }

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
                room.SetTeamWon(team.Flip());
                _finished = true;
            }
        }

        private void OnGameFinished()
        {
            StartCoroutine(EndGame(GameProperties.endGameDelay));
        }

        private void OnGameEnded()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        private IEnumerator EndGame(float delay)
        {
            yield return new WaitForRealSeconds(delay);
            OnGameEnded();
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

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetTeamWon(out _))
            {
                OnGameFinished();
            }
        }
        #endregion

        #region Unity methods
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
