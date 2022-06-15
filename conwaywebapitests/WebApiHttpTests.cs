using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace ConwayWebApi.Tests
{
    [TestFixture]
    public class WebApiHttpTests
    {
        private HttpClient mHttpClient;
        private IHost mHost;

        [SetUp]
        public void Setup()
        {
            mHttpClient = new HttpClient();

            mHost = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();
            Task.Run(mHost.Run);
        }

        [TearDown]
        public async Task TearDown()
        {
            if (mHost != null)
            {
                await mHost.StopAsync();
                mHost.Dispose();
                mHost = null;
            }

            mHttpClient.Dispose();
            mHttpClient = null;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}