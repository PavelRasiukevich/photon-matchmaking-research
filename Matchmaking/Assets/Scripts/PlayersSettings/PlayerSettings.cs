using System;

namespace Assets.Scripts.PlayersSettings
{
    [Serializable]
    public class PlayerSettings
    {
        public int MMR { get; set; }

        public bool IsSearchingForPlayers { get; set; } = false;
    }
}