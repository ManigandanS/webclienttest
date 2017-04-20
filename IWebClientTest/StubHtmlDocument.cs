using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWebClientTest
{
    public class StubHtmlDocument : IHtmlDocument
    {
        public string Body { get; set; }
    }
}
