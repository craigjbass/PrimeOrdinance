using FluentAssertions;
using NUnit.Framework;

namespace PrimeOrdinance
{
    public class GameTests
    {
        private Game _game;

        [SetUp]
        public void BuildTheGame()
        {
            _game = new Game();
        }

        public class GivenNothingHasHappened : GameTests
        {
            [Test]
            public void LobbyCanBeEmpty()
            {
                var presentableLobby = _game.ShowLobby();
                presentableLobby.Players.Should().BeEmpty();
            }

            [Test]
            public void LobbyCanHaveAPlayer()
            {
                _game.AddPlayer("SeaBass");
                var lobby = _game.ShowLobby();
                lobby.Players.Should().ContainSingle("SeaBass");
            }

            [Test]
            public void LobbyCanHaveTwoPlayers()
            {
                _game.AddPlayer("SeaBass");
                _game.AddPlayer("Esoterox");

                var lobby = _game.ShowLobby();
                lobby.Players.Should().ContainInOrder(
                    "SeaBass", "Esoterox"
                );
            }

            [Test]
            public void ThereAreNoUnits()
            {
                _game.ViewUnits().Units.Should().BeEmpty();
            }
        }
        
        public class GivenTwoPlayers : GameTests
        {
            [SetUp]
            public void SetUpTwoPlayers()
            {
                _game.AddPlayer("SeaBass");
                _game.AddPlayer("touchedbydog");
            }

            [Test]
            public void CanBuildAFactory()
            {
                _game.BuildUnit("factory");
                var units = _game.ViewUnits();
                units.Units.Should().ContainSingle("factory");
            }
            
            [Test]
            public void CanBuildATurret()
            {
                _game.BuildUnit("turret");
                var units = _game.ViewUnits();
                units.Units.Should().ContainSingle("turret");
            }
            
            [Test]
            public void CanBuildATurretAndAFactory()
            {
                _game.BuildUnit("turret");
                _game.BuildUnit("factory");
                var units = _game.ViewUnits();
                units.Units.Should().ContainInOrder("turret", "factory");
            }
        }
    }
}