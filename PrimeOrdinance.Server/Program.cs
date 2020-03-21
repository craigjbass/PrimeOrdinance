using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PrimeOrdinance.Protocols;

namespace PrimeOrdinance.Server
{
    class Program
    {
        const int Port = 50051;

        static void Main(string[] args)
        {
            var game = new Game();
            
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = { Lobby.BindService(new LobbyImpl(game)) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }

    internal class LobbyImpl : Lobby.LobbyBase
    {
        private readonly Game _game;

        public LobbyImpl(Game game)
        {
            _game = game;
        }

        public override Task<PresentableLobby> ViewLobby(Empty request, ServerCallContext context)
        {
            var presentableLobby = new PresentableLobby();
            presentableLobby.PlayerNames.AddRange(_game.ShowLobby().Players);
            return Task.FromResult(presentableLobby);
        }
    }
}