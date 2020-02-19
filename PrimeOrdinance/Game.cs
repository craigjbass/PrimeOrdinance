using System.Collections.Generic;

namespace PrimeOrdinance
{
    public class Game
    {
        private readonly List<string> _players = new List<string>();
        private readonly List<string> _units = new List<string>();

        public class PresentableLobby
        {
            public string[] Players;
        }

        public class PresentableUnits
        {
            public string[] Units;
        }

        public PresentableLobby ShowLobby() =>
            new PresentableLobby
            {
                Players = _players.ToArray()
            };

        public void AddPlayer(string playerName) => _players.Add(playerName);

        public void BuildUnit(string unitType)
        {
            _units.Add(unitType);
        }

        public PresentableUnits ViewUnits()
        {
            return new PresentableUnits()
            {
                Units = _units.ToArray()
            };
        }
    }

}