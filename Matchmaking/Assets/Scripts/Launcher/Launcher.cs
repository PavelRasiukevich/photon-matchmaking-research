using Photon.Pun;
using UnityEngine.UI;

namespace Assets.Scripts.Launcher
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region MONOBEH Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #endregion


        #region PUN Callbacks
        #endregion

        public void FindMatchButtonPressed(Button button)
        {
            button.interactable = false;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}