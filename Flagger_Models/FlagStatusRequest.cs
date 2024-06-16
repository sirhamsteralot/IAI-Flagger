using System;
using System.Collections.Generic;
using System.Text;

namespace Flagger.Models
{
    public class FlagStatusRequest
    {
        public string SessionToken { get; set; }
        public List<FlagModel> Flags { get; set; }
    }
}
