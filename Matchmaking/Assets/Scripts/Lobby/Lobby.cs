using Assets.Scripts.Utils;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace Assets.Scripts.Lobby
{
    public class Lobby : MonoBehaviourPunCallbacks
    {

        private Dictionary<string,RoomInfo> _rooms;

        #region PUN CALLBACKS
        public override void OnJoinedLobby()
        {
            MessagesUtilities.JoinLobbyMessage();
            _rooms = new Dictionary<string, RoomInfo>();
        }

        public override void OnLeftLobby()
        {
            MessagesUtilities.LeftLobbyMessage();
        }
        #endregion
    }
}