using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Flagger.Models
{
    public class LobbySettings
    {
        public string Name { get; set; }
        public int NumberOfFlags { get; set; }
        public int MaxUsers { get; set; }

        public Guid Guid { get; set; }
    }
}
