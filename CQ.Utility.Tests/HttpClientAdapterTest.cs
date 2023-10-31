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
        private HttpClientAdapter firebaseApi;

        private HttpClientAdapter authProviderApi;

        [TestInitialize]
        public void OnInit()
        {
            firebaseApi = new HttpClientAdapter(
                "https://identitytoolkit.googleapis.com",
                new List<Header> { new("Referer", "localhost:7049") });
            authProviderApi = new HttpClientAdapter("http://localhost:7049");
        }

        #region Firebase api
        [TestMethod]
        public async Task WhenEmailNotFound_ThenErrorIsReturned()
        {
            await firebaseApi.PostAsync<FirebaseSuccessBody, FirebaseErrorBody>("v1/accounts:signInWithCustomToken?key=", new { email = "some@email.com", password = "123456" }).ConfigureAwait(false);
        }
        #endregion

        #region Auth Provider api
        [TestMethod]
        [ExpectedException(typeof(ConnectionRefusedException))]
        public async Task GivenApiShutDown_WhenExecuteHealthRequest_ThenError()
        {
            try
            {
                await authProviderApi.GetAsync<AuthProviderHealthSuccessBody, AuthProviderErrorBody>("health").ConfigureAwait(false);
            }
            catch (ConnectionRefusedException ex)
            {
                Assert.AreEqual("localhost:7049", ex.Connection);
                throw;
            }
        }

        [TestMethod]
        public async Task GivenApiUp_WhenExecuteHealthRequest_ThenOk()
        {
            var response = await authProviderApi.GetAsync<AuthProviderHealthSuccessBody, AuthProviderErrorBody>("health").ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Alive);
        }
        #endregion
    }

    internal sealed record class FirebaseSuccessBody
    {
        public string Name { get; set; }
    }

    internal sealed record class FirebaseErrorBody
    {
        public Error Error { get; set; }
    }

    internal sealed record class Error
    {
        public string Message { get; set; }
    }

    internal sealed record class AuthProviderHealthSuccessBody
    {
        public bool Alive { get; set; }
    }

    internal sealed record class AuthProviderErrorBody
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
