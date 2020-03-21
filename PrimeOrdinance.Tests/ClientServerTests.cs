using NUnit.Framework;
using PrimeOrdinance.ConsoleClient;
using PrimeOrdinance.Server;

namespace PrimeOrdinance.Tests
{
    public class ClientServerTests
    {
        [Test]
        public void CanSetGameInfo()
        {
            var game = new Game();
            var server = new GameServer(game);

            server.Start();

            var adminPlayerId = server.GetAdminPlayerId();
            var adminClient = new GameClient();
            adminClient.UsePlayerId(adminPlayerId);
            
            
                
        }
    }
}