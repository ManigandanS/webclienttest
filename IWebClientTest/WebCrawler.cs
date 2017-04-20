using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace IWebClientTest
{
    public class WebCrawler : IWebCrawler
    {
        private static SemaphoreSlim downloadSemaphore = new SemaphoreSlim(1);
        private static SemaphoreSlim parseSemaphore = new SemaphoreSlim(1);

        private IWebClient webClient;
        private IHtmlParser parser;

        public WebCrawler(IWebClient webClient, IHtmlParser parser)
        {
            this.webClient = webClient ?? throw new ArgumentNullException("webClient");
            this.parser = parser ?? throw new ArgumentNullException("webClient");
        }

        // For testing purposes
        private int inclient = 0;
        private int inparser = 0;

        public ITestOutputHelper Output { get; internal set; }

        public async Task<IHtmlDocument> Get(string url)
        {
            string html = "";
            IHtmlDocument result = null;

            await downloadSemaphore.WaitAsync();
            try
            {
                //Interlocked.Increment(ref inclient);
                html = await webClient.GetStringAsync(url);
            }
            finally
            {
                //Interlocked.Decrement(ref inclient);
                //Output.WriteLine(inclient.ToString());
                downloadSemaphore.Release();
            }

            await parseSemaphore.WaitAsync();
            try
            {
                //Interlocked.Increment(ref inparser);
                result = parser.Parse(html);
            }
            finally
            {
                //Interlocked.Decrement(ref inparser);
                //Output.WriteLine(inclient.ToString());
                parseSemaphore.Release();
            }

            return result;
        }
    }
}
