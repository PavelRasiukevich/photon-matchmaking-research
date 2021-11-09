using Assets.Scripts.PlayersSettings;
using Assets.Scripts.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MatchMaker
{
    public class MatchMaker : MonoBehaviourPunCallbacks
    {

        #region EXPOSED IN INSPECTOR
        [SerializeField] private byte _maxPlayersPerRoom;
        [SerializeField] private int _initialMMR;

        [Tooltip("How many times we change MMR limitations.")]
        [SerializeField] private int _roomSearchDepth;
        #endregion

        #region PRIVATE FIELDS
        private PlayerSettings _settings;
        private Hashtable _customRoomProperties;
        private Dictionary<string, RoomInfo> _listOfRoomsInfo;
        #endregion

        #region MONOBEH CALLBACKS
        #endregion

        #region PUN CALLBACKS
        public override void OnConnectedToMaster()
        {
            _settings = new PlayerSettings
            {
                //replace with PlayerPrefs.GetInt()
                MMR = _initialMMR,
            };

            _customRoomProperties = new Hashtable
            {
                { UtilsConst.LowerBound, _settings.MMR - UtilsConst.Difference },
                { UtilsConst.UpperBound, _settings.MMR + UtilsConst.Difference }
            };

            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {

        }

        public override void OnJoinedLobby()
        {
            print("Joined Lobby");
            _listOfRoomsInfo = new Dictionary<string, RoomInfo>();
        }

        public override void OnLeftLobby()
        {
            print("Left Lobby");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            RoomListUpdated(roomList);
        }

        public override void OnJoinedRoom()
        {
            //OnJoinedRoom invoke on local client
            print("Joined room.");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //OnPlayerEnteredRoom invokes on other clients
            print("Intered room.");
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);

        #endregion

        #region PUBLIC METHODS
        private void RoomListUpdated(List<RoomInfo> roomList)
        {
            UpdateCachedRoomList(roomList);

            if (_listOfRoomsInfo != null && _listOfRoomsInfo.Count != 0)
                MatchPlayers(_settings, _listOfRoomsInfo);
            else
                CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);
        }
        #endregion

        #region PRIVATE METHODS
        private void MatchPlayers(PlayerSettings playerSettings, Dictionary<string, RoomInfo> roomsInfoList)
        {

            for (int t = _roomSearchDepth - 1; t >= 0; t--)
            {
                foreach (var value in roomsInfoList.Values)
                {
                    var room = value;

                    if (CheckRoomConnectionConditions(room, playerSettings))
                    {
                        playerSettings.ResetChangedMMR();
                        PhotonNetwork.JoinRoom(room.Name);
                        return;
                    }
                }

                playerSettings.ExpandMMRBoundaries(UtilsConst.Difference);
            }

            JoinRandomRoom();
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = roomList.Count - 1; i >= 0; i--)
            {
                RoomInfo info = roomList[i];

                if (!info.RemovedFromList)
                    _listOfRoomsInfo.Add(info.Name, info);
                else
                    _listOfRoomsInfo.Remove(info.Name);
            }
        }

        private void CreateRoomWithCustomOptions(byte maxPlayers, Hashtable customOptions = null)
        {
            print("Create with properties");

            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = maxPlayers,
                CustomRoomPropertiesForLobby = new string[2] { UtilsConst.LowerBound, UtilsConst.UpperBound },
                CustomRoomProperties = customOptions,
                IsOpen = true,
                IsVisible = true
            };

            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        private void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

        private bool CheckRoomConnectionConditions(RoomInfo info, PlayerSettings playerSettings)
        {
            #region Cache
            var customProperties = info.CustomProperties;
            var cachedLowerBound = (int)info.CustomProperties[UtilsConst.LowerBound];
            var cachedUpperBound = (int)info.CustomProperties[UtilsConst.UpperBound];
            var increasedMMR = playerSettings.IncreasedMMR;
            var decreasedMMR = playerSettings.DecreasedMMR;
            #endregion

            if (customProperties == null || customProperties.Count == 0) return false;
            if (!customProperties.ContainsKey(UtilsConst.LowerBound)) return false;
            if (!customProperties.ContainsKey(UtilsConst.UpperBound)) return false;

            return (increasedMMR >= cachedLowerBound
                                        && increasedMMR <= cachedUpperBound)
                                        || (decreasedMMR >= cachedLowerBound
                                        && decreasedMMR <= cachedUpperBound);
        }

        #endregion
    }
}