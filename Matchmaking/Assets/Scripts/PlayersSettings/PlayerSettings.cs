using System;

namespace Assets.Scripts.PlayersSettings
{
    [Serializable]
    public class PlayerSettings
    {
        private int _mmr;
        public int MMR
        {
            get => _mmr;

            set
            {
                _mmr = value;
                ResetChangedMMR();
            }
        }

        public int DecreasedMMR { get; private set; }
        public int IncreasedMMR { get; private set; }

        public bool IsSearchingForPlayers { get; set; } = false;

        public void ResetChangedMMR()
        {
            DecreasedMMR = MMR;
            IncreasedMMR = MMR;
        }

        public void ExpandMMRBoundaries(int delta)
        {
            DecreasedMMR = MMR - delta;
            IncreasedMMR = MMR + delta;
        }
    }
}