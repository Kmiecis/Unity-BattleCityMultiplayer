using Common.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.UI
{
    public class RoomListController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public RoomListEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public Transform[] EntryContainers { get; private set; }
        [field: SerializeField]
        public SelectionController SelectionController { get; private set; }

        private List<RoomInfo> _rooms = new List<RoomInfo>();
        private List<RoomListEntry> _entries = new List<RoomListEntry>();

        private List<RoomInfo> CacheRooms(List<RoomInfo> rooms)
        {
            _rooms.Clear();

            foreach (var room in rooms)
            {
                if (room.IsOpen && room.IsVisible && !room.RemovedFromList)
                {
                    _rooms.Add(room);
                }
            }

            return _rooms;
        }

        private void ClearEntries()
        {
            foreach (var entry in _entries)
            {
                SelectionController.Events.Remove(entry.SelectionEvent);

                entry.gameObject.Destroy();
            }
            _entries.Clear();
        }

        private void CreateEntries(List<RoomInfo> rooms)
        {
            var count = Mathf.Min(rooms.Count, EntryContainers.Length);
            for (int i = 0; i < count; ++i)
            {
                var room = rooms[i];

                var entry = Instantiate(EntryPrefab);
                entry.transform.SetParent(EntryContainers[i], false);
                entry.Setup(room);
                _entries.Add(entry);

                SelectionController.Events.Insert(0, entry.SelectionEvent);
            }
            if (rooms.Count > 0)
            {
                SelectionController.TryChangeCurrent(0);
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearEntries();
            var rooms = CacheRooms(roomList);
            CreateEntries(rooms);
        }

        public override void OnJoinedLobby()
        {
            // Whenever this joins a new lobby, clear any previous room lists
            _rooms.Clear();
            ClearEntries();
        }

        // Note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
        public override void OnLeftLobby()
        {
            _rooms.Clear();
            ClearEntries();
        }

        public override void OnJoinedRoom()
        {
            // Joining (or Entering) a room invalidates any cached lobby room list
            // (even if LeaveLobby was not called due to just joining a room)
            _rooms.Clear();
        }
    }
}
