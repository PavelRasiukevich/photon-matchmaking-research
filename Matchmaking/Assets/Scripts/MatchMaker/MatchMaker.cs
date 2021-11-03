using Assets.Scripts.PlayersSettings;
using Assets.Scripts.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MatchMaker
{
    public class MatchMaker : MonoBehaviourPunCallbacks
    {

        #region Exposed in Inspector Fields
        [SerializeField] private byte _maxPlayersPerRoom;
        [SerializeField] private int _initialMMR;

        [Tooltip("How many times we change MMR limitations.")]
        [SerializeField] private int _roomSearchDepth;
        #endregion

        #region Private Fields
        private PlayerSettings _settings;
        private Hashtable _customRoomProperties;
        private List<RoomInfo> _listOfRoomsInfo;
        #endregion

        #region MONOBEH Callbacks
        #endregion

        #region PUN Callbacks
        public override void OnConnectedToMaster()
        {
            _settings = new PlayerSettings
            {
                //replace with PlayerPrefs.GetInt()
                MMR = _initialMMR,
            };

            _customRoomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            _customRoomProperties.Add(UtilsConst.LowerBound, _settings.MMR - UtilsConst.Difference);
            _customRoomProperties.Add(UtilsConst.UpperBound, _settings.MMR + UtilsConst.Difference);

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            print($"Joined Lobby");
        }

        public override void OnJoinedRoom()
        {
            print($"Joined Room");
            print($"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");

            //load level for all in room

            if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayersPerRoom)
            {
                PhotonNetwork.LoadLevel(UtilsConst.Battle);
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {

            UpdateCachedRoomList(roomList);

            if (_listOfRoomsInfo != null && _listOfRoomsInfo.Count != 0)
                MatchPlayers(_settings, _listOfRoomsInfo);
            else
                CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);

        #endregion

        #region Private Methods
        private void MatchPlayers(PlayerSettings playerSettings, List<RoomInfo> rooms)
        {

            for (int t = _roomSearchDepth - 1; t >= 0; t--)
            {
                for (int i = rooms.Count - 1; i >= 0; i--)
                {
                    var room = rooms[i];

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
            //rewrite
            //method return List<RoomInfo>
            //research how room list behaves
            //when left or enter room

            _listOfRoomsInfo = roomList;

            for (int i = roomList.Count - 1; i >= 0; i--)
            {
                RoomInfo info = roomList[i];

                if (!info.RemovedFromList)
                    _listOfRoomsInfo[i] = info;
                else
                    _listOfRoomsInfo.Remove(info);
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