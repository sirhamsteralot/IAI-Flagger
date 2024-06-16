using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flagger.Client
{
    public interface IHttpClientFactory
    {
        public HttpClient Create();
    }
}
