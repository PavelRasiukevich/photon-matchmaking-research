using Assets.Scripts.PlayersSettings;
using Assets.Scripts.Utils;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Network.Lobby
{
    public class MatchMaker
    {

        public void MatchPlayers(Dictionary<string, RoomInfo> avaliableRooms, PlayerSettings settings)
        {

            for (int t = UtilsConst.SearchDepth - 1; t >= 0; t--)
            {
                foreach (var value in avaliableRooms.Values)
                {
                    var room = value;

                    if (CheckRoomConnectionConditions(room, settings))
                    {
                        settings.ResetChangedMMR();
                        PhotonNetwork.JoinRoom(room.Name);
                        return;
                    }
                }

                settings.ExpandMMRBoundaries(UtilsConst.Difference);
            }

            JoinRandomRoom();
        }

        private bool CheckRoomConnectionConditions(RoomInfo room, PlayerSettings settings)
        {
            #region Cached
            var customProperties = room.CustomProperties;
            var cachedLowerBound = (int)room.CustomProperties[UtilsConst.LowerBound];
            var cachedUpperBound = (int)room.CustomProperties[UtilsConst.UpperBound];
            var increasedMMR = settings.IncreasedMMR;
            var decreasedMMR = settings.DecreasedMMR;
            #endregion

            if (customProperties == null || customProperties.Count == 0) return false;
            if (!customProperties.ContainsKey(UtilsConst.LowerBound)) return false;
            if (!customProperties.ContainsKey(UtilsConst.UpperBound)) return false;

            return increasedMMR >= cachedLowerBound
                                        && increasedMMR <= cachedUpperBound
                                        || decreasedMMR >= cachedLowerBound
                                        && decreasedMMR <= cachedUpperBound;
        }

        private void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    }
}