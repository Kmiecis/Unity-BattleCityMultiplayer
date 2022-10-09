using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Tanks
{
    public class CreateOrJoinRooms : MonoBehaviourPunCallbacks
    {
        private static readonly RoomOptions DefaultRoomOptions = new RoomOptions
        {
            MaxPlayers = 0,
            PlayerTtl = 1000 * 60,
            EmptyRoomTtl = 1000 * 60,
            IsVisible = false,
            IsOpen = true,
            CleanupCacheOnLeave = true
        };
        private static readonly TypedLobby DefaultTypedLobby = TypedLobby.Default;

        public TMP_InputField input;
        public string levelName = "Game";

        public void JoinOrCreateRoom()
        {
            var inputText = input.text;
            var roomName = string.IsNullOrEmpty(inputText) ? "default" : inputText;

            PhotonNetwork.JoinOrCreateRoom(roomName, DefaultRoomOptions, DefaultTypedLobby);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            PhotonNetwork.LoadLevel(levelName);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            // TODO
        }
    }
}
