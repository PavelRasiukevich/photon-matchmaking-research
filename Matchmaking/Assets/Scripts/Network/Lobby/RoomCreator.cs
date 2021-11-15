using Assets.Scripts.Utils;
using Photon.Pun;
using Photon.Realtime;

namespace Assets.Scripts.Network.Lobby
{
    public class RoomCreator
    {
        public void CreateRoomWithCustomOptions(RoomProperties _customPropeties)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = _customPropeties.MaxPlayers,
                CustomRoomPropertiesForLobby = new string[2] { UtilsConst.LowerBound, UtilsConst.UpperBound },
                CustomRoomProperties = _customPropeties.PTSRange,
                IsOpen = true,
                IsVisible = true
            };

            PhotonNetwork.CreateRoom(null, roomOptions);
        }
    }
}