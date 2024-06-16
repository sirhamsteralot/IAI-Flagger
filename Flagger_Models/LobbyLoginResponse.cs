using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Flagger.Models
{
    public class LobbyLoginResponse
    {
        public enum ResponseType
        {
            Rejected_Other,
            Rejected_Full,
            Rejected_InvalidLogin,
            Accepted
        }

        public ResponseType Response { get; set; }
        public string SessionToken { get; set; }
    }
}
