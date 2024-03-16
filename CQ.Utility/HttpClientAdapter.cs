using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class HttpClientAdapter
    {
        private readonly HttpClient _httpClient;

        private delegate Task<HttpResponseMessage> RequestAsync();

        public HttpClientAdapter()
        {
            _httpClient = new();
        }

        public HttpClientAdapter(string baseUrl, List<Header>? baseHeaders = null)
        {
            baseHeaders ??= new List<Header>();

            _httpClient = new()
            {
                BaseAddress = new Uri(baseUrl),
            };

            baseHeaders.ForEach(baseHeader =>
            {
                _httpClient.DefaultRequestHeaders.Add(baseHeader.Name, baseHeader.Value);
            });
        }

        public HttpClientAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Adds the header to the httpClient as default. Befor of adding them, they are removed.
        /// </summary>
        /// <param name="headers"></param>
        public void AddDefaultHeaders(List<Header> headers)
        {
            headers.ForEach(header =>
            {
                var existHeader = _httpClient.DefaultRequestHeaders.Contains(header.Name);

                if (existHeader)
                    _httpClient.DefaultRequestHeaders.Remove(header.Name);

                _httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            });
        }

        /// <summary>
        /// Removes the header from the httpClient.
        /// </summary>
        /// <param name="headers"></param>
        public void RemoveDefaultHeaders(List<Header> headers)
        {
            headers.ForEach(header =>
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Name);
            });
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
            List<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            var request = async () =>
            {
                var response = await _httpClient.PostAsJsonAsync(uri, value).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            return await ExecuteRequest<TSuccessBody, TErrorBody>(requestAsync, processError, headers).ConfigureAwait(false);
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
            List<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            var request = async () =>
            {
                var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            return await ExecuteRequest<TSuccessBody, TErrorBody>(requestAsync, processError, headers).ConfigureAwait(false);
        }

        public virtual async Task<TSuccessBody> DeleteAsync<TSuccessBody, TErrorBody>(
            string uri,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            var request = async () =>
            {
                var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            return await ExecuteRequest<TSuccessBody, TErrorBody>(requestAsync, processError, headers).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync<TErrorBody>(
            string uri,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TErrorBody : class
        {
            var request = async () =>
            {
                var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            await ExecuteRequest(requestAsync, processError, headers).ConfigureAwait(false);
        }

        public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody, TErrorBody>(
            string uri,
            object? value,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            var request = async () =>
            {
                StringContent? content = null;

                if (value != null)
                {
                    var jsonContent = JsonConvert.SerializeObject(value);

                    content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.PutAsync(uri, null).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            return await ExecuteRequest<TSuccessBody, TErrorBody>(requestAsync, processError, headers).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync<TErrorBody>(
            string uri,
            object? value,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TErrorBody : class
        {
            var request = async () =>
            {
                StringContent? content = null;

                if (value != null)
                {
                    var jsonContent = JsonConvert.SerializeObject(value);

                    content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.PutAsync(uri, null).ConfigureAwait(false);

                return response;
            };

            var requestAsync = new RequestAsync(request);

            await ExecuteRequest(requestAsync, processError, headers).ConfigureAwait(false);
        }

        private async Task<TSuccessBody> ExecuteRequest<TSuccessBody, TErrorBody>(
            RequestAsync RequestAsync,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            try
            {
                headers ??= new List<Header>();

                AddDefaultHeaders(headers);

                var response = await RequestAsync().ConfigureAwait(false);

                RemoveDefaultHeaders(headers);

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

        private async Task ExecuteRequest<TErrorBody>(
            RequestAsync RequestAsync,
            Func<TErrorBody, Exception?>? processError = null,
            List<Header>? headers = null)
            where TErrorBody : class
        {
            try
            {
                headers ??= new List<Header>();

                AddDefaultHeaders(headers);

                var response = await RequestAsync().ConfigureAwait(false);

                RemoveDefaultHeaders(headers);

                await ProcessResponseAsync(response, processError).ConfigureAwait(false);
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

        private async Task<TSuccessBody> ProcessResponseAsync<TSuccessBody, TErrorBody>(
            HttpResponseMessage response,
            Func<TErrorBody, Exception?>? processErrorResponse = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            await ProcessErrorAsync(response, processErrorResponse).ConfigureAwait(false);

            var successBody = await this.ProcessBodyAsync<TSuccessBody>(response).ConfigureAwait(false);

            return successBody;
        }

        private async Task ProcessResponseAsync<TErrorBody>(
            HttpResponseMessage response,
            Func<TErrorBody, Exception?>? processErrorResponse = null)
            where TErrorBody : class
        {
            await ProcessErrorAsync(response, processErrorResponse).ConfigureAwait(false);
        }

        private async Task ProcessErrorAsync<TErrorBody>(HttpResponseMessage response, Func<TErrorBody, Exception?>? processErrorResponse) where TErrorBody : class
        {
            if (response.IsSuccessStatusCode)
                return;

            var concreteErrorBody = await this.ProcessBodyAsync<TErrorBody>(response).ConfigureAwait(false);

            if (concreteErrorBody == null)
            {
                var errorBody = await this.ProcessBodyAsync<object>(response).ConfigureAwait(false);

                throw new RequestException<object>(errorBody);
            }

            Exception? exception = null;
            if (processErrorResponse != null)
                exception = processErrorResponse(concreteErrorBody);

            exception ??= new RequestException<TErrorBody>(concreteErrorBody);

            throw exception;
        }

        private async Task<TBody> ProcessBodyAsync<TBody>(HttpResponseMessage response)
            where TBody : class
        {
            TBody? body;
            try
            {
                body = await response.Content.ReadFromJsonAsync<TBody>().ConfigureAwait(false);
            }
            catch
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                body = JsonConvert.DeserializeObject<TBody>(responseAsString);
            }

            if (body == null)
                throw new BodyNullException();

            return body;
        }
    }
}
