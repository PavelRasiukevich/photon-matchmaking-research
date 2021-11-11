using Photon.Realtime;

namespace Assets.Scripts.Rooms
{
    public class CustomRoom : Room
    {
        public string CustomName { get; set; }

        public CustomRoom(string roomName, RoomOptions options, bool isOffline = false) : base(roomName, options, isOffline)
        {
            CustomName = roomName;
        }
    }
}