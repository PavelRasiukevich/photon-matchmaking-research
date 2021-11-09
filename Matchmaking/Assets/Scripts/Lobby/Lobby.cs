using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Lobby
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MatchMaker.MatchMaker _matchMaker;

        public override void OnJoinedLobby()
        {
            print("Joined Lobby");
        }

        public override void OnLeftLobby()
        {
            print("Lobby left");
        }

        
    }
}