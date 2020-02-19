using System;
using FluentAssertions;
using NUnit.Framework;

namespace PrimeOrdinance.Tests
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
            public void ThereAreNoUnits()
            {
                _game.ViewUnits().Units.Should().BeEmpty();
            }

            [Test]
            public void ThereAreNoPlayers()
            {
                var presentableLobby = _game.ShowLobby();
                presentableLobby.Players.Should().BeEmpty();
            }

            [Test]
            public void APlayerCanJoin()
            {
                _game.AddPlayer("SeaBass");
                var lobby = _game.ShowLobby();
                lobby.Players.Should().ContainSingle("SeaBass");
            }

            [Test]
            public void TwoPlayersCanJoin()
            {
                _game.AddPlayer("SeaBass");
                _game.AddPlayer("Esoterox");

                var lobby = _game.ShowLobby();
                lobby.Players.Should().ContainInOrder(
                    "SeaBass", "Esoterox"
                );
            }
        }

        public class GivenTwoPlayers : GameTests
        {
            private string _seaBassId;
            private string _touchedByDogId;

            [SetUp]
            public void TwoPlayersJoin()
            {
                _seaBassId = _game.AddPlayer("SeaBass");
                _touchedByDogId = _game.AddPlayer("touchedbydog");
            }

            [Test]
            public void CanBuildAFactoryOwnedByNobody()
            {
                var id = _game.BuildUnit("factory");
                _game.ViewUnits().Units.Should().BeEquivalentTo(new
                {
                    Id = id.ToGuid(),
                    Type = "factory",
                    OwnedBy = (Guid?) null
                });
            }

            [Test]
            public void CanBuildATurretAndAFactory()
            {
                var id1 = _game.BuildUnit("turret", _seaBassId);
                var id2 = _game.BuildUnit("factory");

                _game.ViewUnits().Units.Should().BeEquivalentTo(
                    new
                    {
                        Id = id1.ToGuid(),
                        Type = "turret",
                        OwnedBy = _seaBassId.ToGuid()
                    },
                    new
                    {
                        Id = id2.ToGuid(),
                        Type = "factory"
                    }
                );
            }

            [Test]
            public void CanBuildAndDestroyATurret()
            {
                var id = _game.BuildUnit("turret");
                _game.DestroyUnit(id);

                _game.ViewUnits().Units.Should().BeEmpty();
            }

            [Test]
            public void CanBuildTurretFactoryTurretAndDestroyTheLastTurret()
            {
                var id1 = _game.BuildUnit("turret", _touchedByDogId);
                var id2 = _game.BuildUnit("factory", _seaBassId);
                var id3 = _game.BuildUnit("turret");
                _game.DestroyUnit(id3);

                _game.ViewUnits().Units.Should().BeEquivalentTo(
                    new
                    {
                        Id = id1.ToGuid(),
                        Type = "turret",
                        OwnedBy = _touchedByDogId.ToGuid()
                    }, new
                    {
                        Id = id2.ToGuid(),
                        Type = "factory",
                        OwnedBy = _seaBassId.ToGuid()
                    }
                );
            }
        }
    }
}