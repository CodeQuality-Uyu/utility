using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class HttpClientAdapter
    {
        private readonly HttpClient _httpClient;

        public HttpClientAdapter()
        {
            _httpClient = new();
        }

        public HttpClientAdapter(string baseUrl, IList<(string name, string value)>? baseHeaders = null)
        {
            _httpClient = new()
            {
                BaseAddress = new Uri(baseUrl),
            };

            if (baseHeaders != null)
            {
                foreach (var baseHeader in baseHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(baseHeader.name, baseHeader.value);
                }
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
            Action<TErrorBody>? processError = null,
            IList<(string name, string value)>? headers = null)
            where TSuccessBody : class
            where TErrorBody : class
        {

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.name, header.value);
                }
            }

            var response = await _httpClient.PostAsJsonAsync(uri, value).ConfigureAwait(false);

            return await ProcessResponseAsync<TSuccessBody, TErrorBody>(response, processError).ConfigureAwait(false);
        }

        private async Task<TSuccessBody> ProcessResponseAsync<TSuccessBody, TErrorBody>(HttpResponseMessage response, Action<TErrorBody>? processErrorResponse = null)
            where TSuccessBody : class
            where TErrorBody : class
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await this.ProcessBodyAsync<TErrorBody>(response).ConfigureAwait(false);

                processErrorResponse?.Invoke(errorBody);

                throw new RequestException<TErrorBody>(errorBody);
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
            catch { 
            
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<TBody>(responseAsString);
            }
        }
    }
}
