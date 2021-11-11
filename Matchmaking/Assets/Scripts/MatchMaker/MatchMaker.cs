using Assets.Scripts.PlayersSettings;
using Assets.Scripts.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MatchMaker
{
    public class MatchMaker : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMPro.TextMeshProUGUI _textMesh;
        [SerializeField] private List<Player> _listOfPlayers;

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
            UtilsMessages.ConnectedToMasterMessage();

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

        public override void OnJoinedLobby()
        {
            UtilsMessages.JoinLobbyMessage();
            _listOfRoomsInfo = new Dictionary<string, RoomInfo>();
        }

        public override void OnLeftLobby()
        {
            UtilsMessages.LeftLobbyMessage();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UtilsMessages.RoomListUpdateMessage();

            var updatedListOfRooms = UpdateCachedRoomList(roomList);

            if (updatedListOfRooms != null && updatedListOfRooms.Count != 0)
                MatchPlayers(_settings, updatedListOfRooms);
            else
                CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);
        }

        //OnJoinedRoom invoke on local client
        public override void OnJoinedRoom()
        {
            UtilsMessages.JoinRoomMessage();

            var playersInRoom = PhotonNetwork.PlayerListOthers;

            _listOfPlayers = new List<Player>();
            _listOfPlayers.AddRange(playersInRoom);
            _listOfPlayers.Add(PhotonNetwork.LocalPlayer);

            _textMesh.text = $"Player in list: {_listOfPlayers.Count}";

        }

        public override void OnLeftRoom()
        {
            UtilsMessages.PlayerLeftRoomMessage(1);
            _listOfPlayers.Clear();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        //OnPlayerEnteredRoom invokes on other clients
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UtilsMessages.PlayerEnterRoomMessage();

            _listOfPlayers.Add(newPlayer);
            _textMesh.text = $"Player in list: {_listOfPlayers.Count}";

            if (_listOfPlayers.Count < 3) return;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            //TODO выключать кнопку отмены поиска

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _listOfPlayers.Count; i++)
                {
                    if (i > 1)
                        photonView.GetComponent<PhotonView>().RPC(nameof(KickPlayerFromRoom), _listOfPlayers[i]);
                }
            }

            _listOfPlayers.Remove(newPlayer);

            PhotonNetwork.LoadLevel(UtilsConst.Battle);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UtilsMessages.PlayerLeftRoomMessage();

            if (_listOfPlayers.Count > 0 && _listOfPlayers != null)
            {
                _listOfPlayers.Remove(otherPlayer);
                _textMesh.text = $"Player in list: {_listOfPlayers.Count}";
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoomWithCustomOptions(_maxPlayersPerRoom, _customRoomProperties);

        public override void OnDisconnected(DisconnectCause cause)
        {
            UtilsMessages.DisconnectedFromMasterMessage();
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

        private Dictionary<string, RoomInfo> UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            for (int i = roomList.Count - 1; i >= 0; i--)
            {
                RoomInfo info = roomList[i];

                if (!info.RemovedFromList)
                    _listOfRoomsInfo.Add(info.Name, info);
                else
                    _listOfRoomsInfo.Remove(info.Name);
            }

            return _listOfRoomsInfo;
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

        [PunRPC]
        private void KickPlayerFromRoom()
        {
            print("Player Kicked From Room");
            PhotonNetwork.LeaveRoom();
        }

        private bool CheckRoomConnectionConditions(RoomInfo info, PlayerSettings playerSettings)
        {
            #region Cached
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