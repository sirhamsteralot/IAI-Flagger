using Flagger.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flagger.Models
{
    public class CreateLobbyModel
    {
        public FlaggerClient FlaggerClient { get; set; }


        public string LobbyName { get; set; } = "";
        public string LobbyPass { get; set; } = "";


        private int maxUsers = 10;
        public string MaxUsers { get { return maxUsers.ToString(); } set { int.TryParse(value, out maxUsers); } }
        private int flagCount = 5;
        public string FlagCount { get { return flagCount.ToString(); } set { int.TryParse(value, out flagCount); } }

        public ICommand CancelCommand { get; set; }
        public ICommand CreateCommand { get; set; }

        public CreateLobbyModel()
        {
            CreateCommand = new Command(execute: async () => {
                var guid = Guid.NewGuid();
                if (await FlaggerClient.CreateLobby(new LobbySettings { Guid = guid, MaxUsers = maxUsers, NumberOfFlags = flagCount, Name = LobbyName }, LobbyPass, guid.ToString())) 
                {
                    bool returnValue = await FlaggerClient.EnterLobby(LobbyPass, guid);
                    if (returnValue)
                    {
                        await Shell.Current.GoToAsync("//" + nameof(LobbyPage), true, new Dictionary<string, object> { { "Client", FlaggerClient } });
                    } else
                    {
                        await Shell.Current.DisplayAlert("Can't connect to server", "Check internet connection", "ok");
                    }
                } else
                {
                    await Shell.Current.DisplayAlert("Can't connect to server", "Check internet connection", "ok");
                }
            });

            CancelCommand = new Command(async () => {
                await Shell.Current.GoToAsync("//" + nameof(MainPage));
            });
        }

    }
}
