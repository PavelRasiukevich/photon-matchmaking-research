using Assets.Scripts.Core;
using Assets.Scripts.Utils;
using Photon.Pun;
using Photon.Realtime;

namespace Assets.Scripts.Network.MasterServer
{
    public class MasterServerObserver : MonoBehaviourPunCallbacks
    {
        #region PUN CALLBACKS
        public override void OnConnectedToMaster()
        {
            MessagesUtilities.ConnectedToMasterMessage();

            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            MessagesUtilities.DisconnectedFromMasterMessage();
            ScreenHolder.SetCurrentScreen(ScreenType.MainMenu).ShowScreen();
        }

        #endregion
    }
}