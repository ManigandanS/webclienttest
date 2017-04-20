using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWebClientTest
{
    public class StubHtmlParser : IHtmlParser
    {
        public IHtmlDocument Parse(string htmlText)
        {
            // Generate work
            for (int i = 0; i < 100000; i++)
            {
                htmlText.Replace("1", "0");
            }

            return new StubHtmlDocument { Body = htmlText };
        }
    }
}
