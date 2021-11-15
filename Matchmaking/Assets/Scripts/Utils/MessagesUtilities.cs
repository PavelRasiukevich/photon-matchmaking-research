using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class MessagesUtilities
    {
        public static void ConnectedToMasterMessage() => Debug.Log("Connected To Master");
        public static void JoinLobbyMessage() => Debug.Log("Lobby Joined");
        public static void LeftLobbyMessage() => Debug.Log("Lobby Left");
        public static void JoinRoomMessage() => Debug.Log("Room Joined");
        public static void LeftRoomMessage() => Debug.Log("Room Left");
        public static void PlayerLeftRoomMessage() => Debug.Log("Remote Player Left Room");
        public static void PlayerLeftRoomMessage(int version) => Debug.Log("Local Player Left Room");
        public static void PlayerEnterRoomMessage() => Debug.Log("Player Entered Room");
        public static void RoomListUpdateMessage() => Debug.Log("Room List Updated");
        public static void DisconnectedFromMasterMessage() => Debug.Log("Disconnected");
    }
}