using Photon.Pun;

namespace Assets.Scripts.Launcher
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region MONOBEH Callbacks

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        #endregion

        #region PUN Callbacks
        #endregion

        public void StartMatching() => PhotonNetwork.ConnectUsingSettings();

        public void StopMatching() => PhotonNetwork.Disconnect();

    }
}