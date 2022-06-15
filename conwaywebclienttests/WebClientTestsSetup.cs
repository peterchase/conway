using NUnit.Framework;
using ConwayWebClient;

namespace ConwayWebClientTests
{
    [SetUpFixture]
    public class WebClientTestsSetup
    {
        [OneTimeSetUp]
        public void SetupClientTests()
        {
            ConwayClient.SetupClient("http://localhost:5000/");
        }

    }
}