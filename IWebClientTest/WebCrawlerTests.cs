using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IWebClientTest
{
    public class WebCrawlerTests
    {
        private readonly ITestOutputHelper output;

        public WebCrawlerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private Fixture fixture = new Fixture();

        [Fact]
        public async void TestConcurrency()
        {
            Debug.Listeners.Add(new DefaultTraceListener());

            var parser = new StubHtmlParser();
            var webclient = new StubWebClient();

            List<string> results = new List<string>();

            var crawler = new WebCrawler(webclient, parser);
            crawler.Output = output;

            List<Task> tasks = new List<Task>();
            for(int i = 0; i < 100; i++)
            {
                tasks.Add(GetResult(crawler, results));
            }

            await Task.WhenAll(tasks);

            // Assert there are no duplicates
            Assert.Equal(100, results.Distinct().Count());
        }

        public async Task GetResult(IWebCrawler crawler, IList<string> results)
        {
            var result = await crawler.Get(fixture.Create<string>());
            output.WriteLine(result.Body);
            results.Add(result.Body);
        }
    }
}
