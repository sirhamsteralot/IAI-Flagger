using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flagger.Models
{
    public class FlagModel
    {
        public enum FlagType
        {
            None,
            Green,
            Yellow,
            Red
        }

        public FlagType CurrentFlag { get; set; } = FlagType.Green;
        public int Number { get; set; }
    }
}

