using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class HttpClientAdapter
    {
        private readonly HttpClient _httpClient;

        private delegate Task<HttpResponseMessage> RequestAsync(string uri);

        public HttpClientAdapter()
        {
            _httpClient = new();
        }

        public HttpClientAdapter(string baseUrl, IList<Header>? baseHeaders = null)
        {
            _httpClient = new()
            {
                BaseAddress = new Uri(baseUrl),
            };

            if (baseHeaders != null)
            {
                foreach (var baseHeader in baseHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(baseHeader.Name, baseHeader.Value);
                }
            }
        }

        /// <summary>
        /// Adds the header to the httpClient as default. Befor of adding them, they are removed.
        /// </summary>
        /// <param name="headers"></param>
        public void AddDefaultHeaders(IList<Header> headers)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Name);
                _httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
        }


        /// <summary>
        /// Removes the header from the httpClient.
        /// </summary>
        /// <param name="headers"></param>
        public void RemoveDefaultHeaders(IList<Header> headers)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Name);
            }
        }

        /// <summary>
        /// Execute post request and parse success body to TSuccessBody and error body to TErrorBody.
        /// </summary>
        /// <typeparam name="TSuccessBody"></typeparam>
        /// <typeparam name="TErrorBody"></typeparam>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <param name="processError"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        /// <exception cref="RequestException{TError}"></exception>
        public virtual async Task<TSuccessBody> PostAsync<TSuccessBody, TErrorBody>(
            string uri,
            object value,
            Func<TErrorBody, Exception?>? processError = null,
            IList<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            var post = async (string uri) =>
            {
                var response = await _httpClient.PostAsJsonAsync(uri, value).ConfigureAwait(false);

                return response;
            };

            var request = new RequestAsync(post);

            return await ExecuteRequest<TSuccessBody, TErrorBody>(uri, request, processError, headers).ConfigureAwait(false);
        }

        private async Task<TSuccessBody> ProcessResponseAsync<TSuccessBody, TErrorBody>(
            HttpResponseMessage response,
            Func<TErrorBody, Exception?>? processErrorResponse = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            if (!response.IsSuccessStatusCode)
            {
                var concreteErrorBody = await this.ProcessBodyAsync<TErrorBody>(response).ConfigureAwait(false);

                if(concreteErrorBody == null)
                {
                    var errorBody = await this.ProcessBodyAsync<object>(response).ConfigureAwait(false);

                    throw new RequestException<object>(errorBody);
                }

                Exception? exception = null;
                if(processErrorResponse != null)
                {
                    exception = processErrorResponse(concreteErrorBody);
                }

                exception ??= new RequestException<TErrorBody>(concreteErrorBody);

                throw exception;
            }

            var successBody = await this.ProcessBodyAsync<TSuccessBody>(response).ConfigureAwait(false);

            return successBody;
        }

        private async Task<TBody> ProcessBodyAsync<TBody>(HttpResponseMessage response)
            where TBody : class
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<TBody>().ConfigureAwait(false);
            }
            catch
            {

                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<TBody>(responseAsString);
            }
        }

        /// <summary>
        /// Execute get request and parse success body to TSuccessBody and error body to TErrorBody.
        /// </summary>
        /// <typeparam name="TSuccessBody"></typeparam>
        /// <typeparam name="TErrorBody"></typeparam>
        /// <param name="uri"></param>
        /// <param name="processError"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual async Task<TSuccessBody> GetAsync<TSuccessBody, TErrorBody>(
            string uri,
            Func<TErrorBody, Exception?>? processError = null,
            IList<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            return await ExecuteRequest<TSuccessBody, TErrorBody>(uri, _httpClient.GetAsync, processError, headers).ConfigureAwait(false);
        }

        private async Task<TSuccessBody> ExecuteRequest<TSuccessBody, TErrorBody>(
            string uri,
            RequestAsync Request,
            Func<TErrorBody, Exception?>? processError = null,
            IList<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            try
            {

                if (headers != null)
                {
                    AddDefaultHeaders(headers);
                }

                var response = await Request(uri).ConfigureAwait(false);

                if (headers != null)
                {
                    RemoveDefaultHeaders(headers);
                }

                return await ProcessResponseAsync<TSuccessBody, TErrorBody>(response, processError).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message.ToLower().StartsWith("no connection could be made"))
                {
                    Match match = Regex.Match(ex.Message, @"\((.*?)\)");

                    string connection;
                    if (match.Success)
                    {
                        connection = match.Groups[1].Value;
                    }
                    else
                    {
                        connection = "-";
                    }

                    throw new ConnectionRefusedException(connection);
                }
                throw;
            }
        }
    }
}
