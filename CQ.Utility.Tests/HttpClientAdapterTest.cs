using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility.Tests
{
    [TestClass]
    public class HttpClientAdapterTest
    {
        private HttpClientAdapter httpClientAdapter;

        [TestInitialize]
        public void OnInit()
        {
            httpClientAdapter = new HttpClientAdapter("https://identitytoolkit.googleapis.com", new List<(string, string)> { ("Referer", "localhost:7049") });
        }

        [TestMethod]
        public async Task WhenEmailNotFound_ThenErrorIsReturned()
        {
            await httpClientAdapter.PostAsync<SuccessBody, ErrorBody>("v1/accounts:signInWithCustomToken?key=", new { email = "some@email.com", password = "123456" }).ConfigureAwait(false);
        }
    }

    internal sealed record class SuccessBody
    {
        public string Name { get; set; }
    }

    internal sealed record class ErrorBody
    {
        public Error Error { get; set; }
    }

    internal sealed record class Error
    {
        public string Message { get; set; }
    }
}
