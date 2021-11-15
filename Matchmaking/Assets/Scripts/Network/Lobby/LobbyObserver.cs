using Assets.Scripts.PlayersSettings;
using Assets.Scripts.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Network.Lobby
{
    [Serializable]
    public class RoomProperties
    {
        [SerializeField] private byte _maxPlayers;

        public byte MaxPlayers => _maxPlayers;

        public Hashtable PTSRange { get; set; }
    }

    public class LobbyObserver : MonoBehaviourPunCallbacks
    {
        #region EXPOSED IN INSPECTOR

        [SerializeField] private int _initialMMR;
        [SerializeField] private RoomProperties _customRoomProperties;
        #endregion

        #region PRIVATE FIELDS
        private Dictionary<string, RoomInfo> _rooms;
        private PlayerSettings _settings;
        #endregion

        #region HELPERS
        private RoomListUpdater _roomListUpdater;
        private MatchMaker _matchMaker;
        private RoomCreator _roomCreator;
        #endregion

        #region MONOBEH CALLBACKS
        private void Awake()
        {
            _roomListUpdater = new RoomListUpdater();
            _matchMaker = new MatchMaker();
            _roomCreator = new RoomCreator();
        }
        #endregion

        #region PUN CALLBACKS
        public override void OnJoinedLobby()
        {
            MessagesUtilities.JoinLobbyMessage();

            _rooms = new Dictionary<string, RoomInfo>();

            #region Potential Refactoring
            _settings = new PlayerSettings
            {
                //replace with PlayerPrefs.GetInt()
                MMR = _initialMMR,
            };

            _customRoomProperties.PTSRange = new Hashtable
             {
                 { UtilsConst.LowerBound, _settings.MMR - UtilsConst.Difference },
                 { UtilsConst.UpperBound, _settings.MMR + UtilsConst.Difference }
             };
            #endregion

        }

        public override void OnLeftLobby() => MessagesUtilities.LeftLobbyMessage();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            MessagesUtilities.RoomListUpdateMessage();

            _rooms = _roomListUpdater.UpdateCachedRoomList(roomList, _rooms);

            if (_rooms != null && _rooms.Count != 0)
                _matchMaker.MatchPlayers(_rooms, _settings);
            else
                _roomCreator.CreateRoomWithCustomOptions(_customRoomProperties);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => _roomCreator.CreateRoomWithCustomOptions(_customRoomProperties);

        public override void OnCreateRoomFailed(short returnCode, string message) => print($"{returnCode} / Message: {message}");

        #endregion
    }
}