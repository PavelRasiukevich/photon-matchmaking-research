using Assets.Scripts.Utils;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace Assets.Scripts.Network.Rooms
{
    public class RoomObserver : MonoBehaviourPunCallbacks
    {
        private List<Player> _playersInRoom;
        private byte _maxPlayers;
        private PhotonView _photonView;

        private void Awake()
        {
           _photonView = photonView.GetComponent<PhotonView>();
        }

        #region PUN CALLBACKS
        public override void OnJoinedRoom()
        {
            MessagesUtilities.JoinRoomMessage();

            GetAllPlayersInCurrentRoom();
            _maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        }

        public override void OnLeftRoom()
        {
            MessagesUtilities.PlayerLeftRoomMessage(1);
            ClearListOfPlayers();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            MessagesUtilities.PlayerEnterRoomMessage();

            _playersInRoom.Add(newPlayer);

            if (_playersInRoom.Count < _maxPlayers) return;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            CheckForExtraPlayers(_playersInRoom);

            if (!PhotonNetwork.IsMasterClient) return;

            PhotonNetwork.LoadLevel(UtilsConst.Battle);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            MessagesUtilities.PlayerLeftRoomMessage();

            if (_playersInRoom.Count <= 0 || _playersInRoom == null) return;

            _playersInRoom.Remove(otherPlayer);
        }

        #endregion

        #region PRIVATE METHODS
        private void ClearListOfPlayers() => _playersInRoom.Clear();

        private void GetAllPlayersInCurrentRoom()
        {
            _playersInRoom = new List<Player>();
            _playersInRoom.AddRange(PhotonNetwork.PlayerList);
        }

        private void CheckForExtraPlayers(List<Player> players)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            for (int i = 0; i < players.Count; i++)
                if (i > 1)
                    _photonView.RPC(nameof(KickPlayerFromRoom), players[i]);
        }

        [PunRPC]
        private void KickPlayerFromRoom() => PhotonNetwork.LeaveRoom();

        #endregion

    }
}