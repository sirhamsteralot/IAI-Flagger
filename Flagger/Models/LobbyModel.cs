using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flagger.Models
{
    public class LobbyModel
    {
        public LobbySettings Settings { get; set; }
        public ICommand JoinLobby => JoinLobbyS;
        public static ICommand JoinLobbyS { get; set; }
    }
}
