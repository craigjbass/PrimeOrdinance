using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PrimeOrdinance.Protocols;

namespace PrimeOrdinance.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new Lobby.LobbyClient(channel);

            var reply = client.ViewLobby(new Empty());
            Console.WriteLine("Greeting: " + reply.PlayerNames);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}