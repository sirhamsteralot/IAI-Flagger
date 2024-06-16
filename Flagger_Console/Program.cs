using Flagger.Client;
using Flagger.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flagger_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var clientFactory = new Flagger.HttpClientFactory();
            FlaggerClient client = new FlaggerClient();
            client.Init("http://localhost:5251", clientFactory);

            Guid guid = Guid.NewGuid();
            string pass = "password";

            while (true) {

                Console.WriteLine("Select option: list, create, login, flags, all");

                switch(Console.ReadLine())
                {
                    case "list":
                        List(client);
                        break;
                    case "create":
                        Create(client, guid, pass);
                        break;
                    case "login":
                        Login(client, guid, pass);
                        break;
                    case "flags":
                        GetFlags(client, guid);
                        break;
                    case "all":
                        List(client);
                        Create(client, guid, pass);
                        List(client);
                        Login(client, guid, pass);
                        GetFlags(client, guid);
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        public static async void List(FlaggerClient client)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            var lobbies = await client.GetLobbies();

            if (lobbies == null)
            {
                Console.WriteLine("No lobbies found");
                return;
            }

            foreach (var lobby in lobbies)
            {
                Console.WriteLine(JsonSerializer.Serialize(lobby, options));
            }
        }

        public static async void Create(FlaggerClient client, Guid guid, string lobbyPassword)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            LobbySettings settings = new LobbySettings { 
                Guid = guid,
                MaxUsers = 10,
                Name = "TestLobby 🥐",
                NumberOfFlags = 3
            };

            Console.WriteLine("Creating lobby with settings:");
            Console.WriteLine(JsonSerializer.Serialize(settings, options));

            var result = await client.CreateLobby(settings, lobbyPassword, guid.ToString());
            Console.WriteLine(result);
        }

        public static async void Login(FlaggerClient client, Guid guid, string password)
        {
            Console.WriteLine($"Logging into lobby {guid}:");
            var result = await client.EnterLobby(password, guid);
            Console.WriteLine(result);
        }


        public static async void GetFlags(FlaggerClient client, Guid guid)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            Console.WriteLine("Getting flag statussy");

            var flags = await client.SyncFlagStatus(new List<FlagModel>());

            Console.WriteLine(JsonSerializer.Serialize(flags, options));
        }
    }
}
