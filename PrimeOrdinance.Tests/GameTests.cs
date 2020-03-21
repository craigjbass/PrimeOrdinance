using System;
using FluentAssertions;
using NUnit.Framework;

namespace PrimeOrdinance.Tests
{
    public class GameTests : TimeProvider
    {
        private Game _game;
        private long _currentGameTime;

        [SetUp]
        public void BuildTheGame()
        {
            _currentGameTime = 0;
            _game = new Game(this);
        }
        
        public long GetTime()
        {
            return _currentGameTime;
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

        public class DescribeBuildingUnits : GameTests
        {
            [Test]
            public void UnitsStartInTheConstructingState()
            {
                var playerId = _game.AddPlayer("SeaBass");

                _game.BuildUnit("factory", playerId);

                _game.ViewUnits().Units[0].State.Should().Be("Constructing");
            }
        }

        public class DescribeTheEconomy : GameTests
        {
            [Test]
            public void TheEconomyStartsAt0Matter()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _game.ViewEconomy(playerId).MatterRate.Should().Be(0);
                _game.ViewEconomy(playerId).Matter.Should().Be(0);
            }
            
            [Test]
            public void AfterBuildingAMatterOriginatorTheMatterRateIs10()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _game.BuildUnit("matter-originator", playerId);
                
                _game.ViewEconomy(playerId).MatterRate.Should().Be(10);
                _game.ViewEconomy(playerId).Matter.Should().Be(0);
            }
            
            [Test]
            public void AfterBuildingAMatterOriginatorAndAfterOneSecondTheMatterIs10()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _game.BuildUnit("matter-originator", playerId);

                _currentGameTime = 1000;
                
                _game.ViewEconomy(playerId).MatterRate.Should().Be(10);
                _game.ViewEconomy(playerId).Matter.Should().Be(10);
            }
            
            [Test]
            public void AfterBuildingAMatterOriginatorAndAfterTwoSecondsTheMatterIs20()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _game.BuildUnit("matter-originator", playerId);

                _currentGameTime = 2000;
                
                _game.ViewEconomy(playerId).MatterRate.Should().Be(10);
                _game.ViewEconomy(playerId).Matter.Should().Be(20);
            }
            
            [Test]
            public void CanBuildAMatterOriginator1SecondIntoGamePlay()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _currentGameTime = 1000;

                _game.BuildUnit("matter-originator", playerId);

                _currentGameTime = 2000;
                
                _game.ViewEconomy(playerId).MatterRate.Should().Be(10);
                _game.ViewEconomy(playerId).Matter.Should().Be(10);
            }
            
            [Test]
            public void CanShowCorrectMatterRateWhenDestroyed()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _currentGameTime = 1000;

                var unitId = _game.BuildUnit("matter-originator", playerId);
                
                _currentGameTime = 2000;
                
                _game.DestroyUnit(unitId);
                
                _game.ViewEconomy(playerId).MatterRate.Should().Be(0);
                _game.ViewEconomy(playerId).Matter.Should().Be(10);
            }
            
            [Test]
            public void CanStopOriginatingMatterWhenDestroyed()
            {
                var playerId = _game.AddPlayer("SeaBass");
                _currentGameTime = 1000;

                var unitId = _game.BuildUnit("matter-originator", playerId);
                
                _currentGameTime = 2000;
                _game.DestroyUnit(unitId);
                
                _currentGameTime = 3000;
                _game.ViewEconomy(playerId).MatterRate.Should().Be(0);
                _game.ViewEconomy(playerId).Matter.Should().Be(10);
            }
        }
    }
}