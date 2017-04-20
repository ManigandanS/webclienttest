using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<IHtmlDocument> Get(string url)
        {
            string html = "";
            IHtmlDocument result = null;

            await downloadSemaphore.WaitAsync();
            try
            {
                html = await webClient.GetStringAsync(url);
            }
            finally
            {
                downloadSemaphore.Release();
            }

            await parseSemaphore.WaitAsync();
            try
            {
                result = parser.Parse(html);
            }
            finally
            {
                parseSemaphore.Release();
            }

            return result;
        }
    }
}
