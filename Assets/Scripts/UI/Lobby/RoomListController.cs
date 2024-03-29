﻿using Common.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.UI
{
    public class RoomListController : MonoBehaviourPunCallbacks
    {
        [field: SerializeField]
        public GameData GameProperties { get; private set; }
        [field: SerializeField]
        public GameObject NoRoomsPanel { get; private set; }
        [field: SerializeField]
        public RoomEntry EntryPrefab { get; private set; }
        [field: SerializeField]
        public Transform EntryParent { get; private set; }
        [field: SerializeField]
        public SelectionController SelectionController { get; private set; }

        private List<RoomInfo> _rooms = new List<RoomInfo>();
        private List<RoomEntry> _entries = new List<RoomEntry>();

        private List<RoomInfo> CacheRooms(List<RoomInfo> rooms)
        {
            _rooms.Clear();

            foreach (var room in rooms)
            {
                _rooms.Add(room);
            }

            return _rooms;
        }

        private void ClearEntries()
        {
            foreach (var entry in _entries)
            {
                entry.gameObject.Destroy();
            }
            _entries.Clear();

            SelectionController.Events.Clear();
        }

        private void CreateEntries(List<RoomInfo> rooms)
        {
            var count = Mathf.Min(rooms.Count, GameProperties.maxRoomsListed);
            for (int i = 0; i < count; ++i)
            {
                var room = rooms[i];

                var entry = Instantiate(EntryPrefab);
                entry.transform.SetParent(EntryParent, false);
                entry.Setup(room);
                _entries.Add(entry);

                SelectionController.Events.Add(entry.SelectionEvent);
            }

            NoRoomsPanel.SetActive(rooms.Count == 0);
            if (rooms.Count > 0)
            {
                SelectionController.Refresh();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            var rooms = CacheRooms(roomList);
            if (isActiveAndEnabled)
            {
                ClearEntries();
                CreateEntries(rooms);
            }
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

        public override void OnEnable()
        {
            base.OnEnable();
            CreateEntries(_rooms);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ClearEntries();
        }
    }
}
