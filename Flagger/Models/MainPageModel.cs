using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Flagger.Client;

namespace Flagger.Models
{
    public class MainPageModel
    {
        public string PasswordField { get; set; } = "";
        public ObservableCollection<LobbyModel> Lobbies { get; set; } = new ObservableCollection<LobbyModel>();
        public ICommand RefreshLobbies { private set; get; }
        public ICommand CreateLobby { private set; get; }
        public ICommand JoinLobby { private set; get; }

        private const string httpRoot = "https://immaterium.nl";

        FlaggerClient flaggerClient { get; set; }

        public MainPageModel()
        {
            flaggerClient = new FlaggerClient();

            HttpClientFactory clientFactory = new HttpClientFactory();

            flaggerClient.Init(httpRoot, clientFactory);

            LobbyModel.JoinLobbyS = new Command<Guid>(execute: async (Guid argument) => {
                bool returnValue = await flaggerClient.EnterLobby(PasswordField, argument);
                if (returnValue)
                {
                    await Shell.Current.GoToAsync("//" + nameof(LobbyPage), true, new Dictionary<string, object> { { "Client",  flaggerClient} });
                } else
                {
                    await AppShell.Current.DisplayAlert("Can't connect to server", "Check login details and internet connection", "ok");
                }
            });

            RefreshLobbies = new Command(execute: async () => {
                try
                {
                    Lobbies.Clear();
                    foreach (var lobbySettings in await flaggerClient.GetLobbies())
                    {
                        var lobbyModel = new LobbyModel();
                        lobbyModel.Settings = lobbySettings;
                        Lobbies.Add(lobbyModel);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    await AppShell.Current.DisplayAlert("Can't connect to server", ex.Message, "ok");

                }
            });

            CreateLobby = new Command(execute: async () => {
                    await Shell.Current.GoToAsync("//" + nameof(CreateLobbyPage), true, new Dictionary<string, object> { { "Client", flaggerClient } });
            });
        }
    }
}
