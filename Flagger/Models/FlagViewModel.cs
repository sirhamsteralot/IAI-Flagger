using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flagger.Models
{
    public class FlagViewModel 
    {
        public Color CurrentColour { get; set; }

        public FlagModel FlagModel { get; set; } 

        public ICommand FlagClicked { get; set; }

        public bool Changed { get; set; }
    }
}
