using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Flagger.Models
{
    public class CreateLobbyRequest
    {
        public string Password { get; set; }
        public LobbySettings LobbySettings { get; set; }
    }
}
