using Photon.Realtime;
using System.Collections.Generic;

namespace Assets.Scripts.Network.Lobby
{
    public class RoomListUpdater
    {
        public Dictionary<string, RoomInfo> UpdateCachedRoomList(List<RoomInfo> updatedRooms, Dictionary<string,RoomInfo> result)
        {
            for (int i = updatedRooms.Count - 1; i >= 0; i--)
            {
                RoomInfo info = updatedRooms[i];

                if (!info.RemovedFromList)
                    result.Add(info.Name, info);
                else
                    result.Remove(info.Name);
            }

            return result;
        }
    }
}