using Photon.Realtime;
using System.Collections.Generic;

namespace Assets.Scripts.PlayersSettings
{
    public class ExtendedPlayer : Player
    {
        public List<ExtendedPlayer> PLayers { get; set; }

        protected internal ExtendedPlayer(string nickName, int actorNumber, bool isLocal) : base(nickName, actorNumber, isLocal)
        {
        }

        protected internal ExtendedPlayer(string nickName, int actorNumber, bool isLocal, ExitGames.Client.Photon.Hashtable playerProperties) : base(nickName, actorNumber, isLocal, playerProperties)
        {
        }
    }
}