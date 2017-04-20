using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWebClientTest
{
    public class StubWebClient : IWebClient
    {
        private Fixture fixture = new Fixture();

        public StubWebClient()
        {

        }

        public async Task<string> GetStringAsync(string urlText)
        {
            // Generate work
            await Task.Run(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    urlText.Replace("1", "0");
                }
            });

            return fixture.Create<string>();
        }
    }
}
