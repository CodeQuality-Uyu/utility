using CQ.Utility.Exceptions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace CQ.Utility;
public class HttpClientAdapter
{
    private readonly HttpClient _httpClient;

    public HttpClientAdapter()
    {
        _httpClient = new();
    }

    public HttpClientAdapter(
        string baseUrl,
        List<Header>? baseHeaders = null)
    {
        baseHeaders ??= [];

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

    public void AddDefaultHeaders(List<Header> headers)
    {
        headers.ForEach(header =>
        {
            var existHeader = _httpClient.DefaultRequestHeaders.Contains(header.Name);

            if (existHeader)
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Name);
            }

            _httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
        });
    }

    public void RemoveDefaultHeaders(List<Header> headers)
    {
        headers.ForEach(header =>
        {
            _httpClient.DefaultRequestHeaders.Remove(header.Name);
        });
    }

    #region Post
    public virtual async Task<TSuccessBody> PostAsync<TSuccessBody, TErrorBody>(
        string uri,
        object value,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> PostRequestAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(uri, value).ConfigureAwait(false);

            return response;
        };

        var successBody = await ExecuteRequest<TSuccessBody, TErrorBody>(PostRequestAsync, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> PostAsync<TSuccessBody, TErrorBody>(
        string uri,
        object value,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        var successBody = await PostAsync<TSuccessBody, TErrorBody>(uri, value, ProcessError<TErrorBody>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task PostVoidAsync<TErrorBody>(
        string uri,
        object value,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> PostRequestAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(uri, value).ConfigureAwait(false);

            return response;
        };

        await ExecuteRequest<TErrorBody>(PostRequestAsync, processError, headers).ConfigureAwait(false);
    }

    public virtual async Task PostVoidAsync<TErrorBody>(
        string uri,
        object value,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        await PostVoidAsync<TErrorBody>(uri, value, ProcessError<TErrorBody>, headers).ConfigureAwait(false);
    }

    public virtual async Task<TSuccessBody> PostAsync<TSuccessBody>(
        string uri,
        object value,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await PostAsync<TSuccessBody, object>(uri, value, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> PostAsync<TSuccessBody>(
        string uri,
        object value,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await PostAsync<TSuccessBody>(uri, value, ProcessError<object>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task PostAsync(
        string uri,
        object value,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
    {
        await PostVoidAsync<object>(uri, value, processError, headers).ConfigureAwait(false);
    }

    public virtual async Task PostAsync(
        string uri,
        object value,
        List<Header>? headers = null)
    {
        await PostAsync(uri, value, ProcessError<object>, headers).ConfigureAwait(false);
    }
    #endregion

    #region Get
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
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> GetAsync()
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            return response;
        };

        var successBody = await ExecuteRequest<TSuccessBody, TErrorBody>(GetAsync, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> GetAsync<TSuccessBody, TErrorBody>(
        string uri,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        var successBody = await GetAsync<TSuccessBody, TErrorBody>(uri, ProcessError<TErrorBody>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> GetAsync<TSuccessBody>(
        string uri,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await GetAsync<TSuccessBody, object>(uri, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> GetAsync<TSuccessBody>(
        string uri,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await GetAsync<TSuccessBody>(uri, ProcessError<object>, headers).ConfigureAwait(false);

        return successBody;
    }
    #endregion

    #region Delete
    public virtual async Task<TSuccessBody> DeleteAsync<TSuccessBody, TErrorBody>(
        string uri,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> DeleteAsync()
        {
            var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);

            return response;
        };

        var successBody = await ExecuteRequest<TSuccessBody, TErrorBody>(DeleteAsync, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> DeleteAsync<TSuccessBody, TErrorBody>(
        string uri,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        var successBody = await DeleteAsync<TSuccessBody, TErrorBody>(uri, ProcessError<TErrorBody>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task DeleteVoidAsync<TErrorBody>(
        string uri,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> DeleteAsync()
        {
            var response = await _httpClient.DeleteAsync(uri).ConfigureAwait(false);

            return response;
        };

        await ExecuteRequest(DeleteAsync, processError, headers).ConfigureAwait(false);
    }

    public virtual async Task DeleteVoidAsync<TErrorBody>(
        string uri,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        await DeleteVoidAsync<TErrorBody>(uri, ProcessError<TErrorBody>, headers).ConfigureAwait(false);
    }

    public virtual async Task<TSuccessBody> DeleteAsync<TSuccessBody>(
        string uri,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await DeleteAsync<TSuccessBody, object>(uri, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> DeleteAsync<TSuccessBody>(
       string uri,
       List<Header>? headers = null)
       where TSuccessBody : class
    {
        var successBody = await DeleteAsync<TSuccessBody>(uri, ProcessError<object>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task DeleteAsync(
        string uri,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
    {
        await DeleteVoidAsync<object>(uri, processError, headers).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(
        string uri,
        List<Header>? headers = null)
    {
        await DeleteAsync(uri, ProcessError<object>, headers).ConfigureAwait(false);
    }
    #endregion

    #region Update
    public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody, TErrorBody>(
        string uri,
        object? value,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> UpdateAsync()
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

        var successBody = await ExecuteRequest<TSuccessBody, TErrorBody>(UpdateAsync, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody, TErrorBody>(
        string uri,
        object? value,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        var successBody = await UpdateAsync<TSuccessBody, TErrorBody>(uri, value, ProcessError<TErrorBody>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody>(
        string uri,
        object? value,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await UpdateAsync<TSuccessBody, object>(uri, value, processError, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody>(
        string uri,
        object? value,
        List<Header>? headers = null)
        where TSuccessBody : class
    {
        var successBody = await UpdateAsync<TSuccessBody>(uri, value, ProcessError<object>, headers).ConfigureAwait(false);

        return successBody;
    }

    public virtual async Task UpdateVoidAsync<TErrorBody>(
        string uri,
        object? value,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        async Task<HttpResponseMessage> UpdateAsync()
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

        await ExecuteRequest(UpdateAsync, processError, headers).ConfigureAwait(false);
    }

    public virtual async Task UpdateVoidAsync<TErrorBody>(
        string uri,
        object? value,
        List<Header>? headers = null)
        where TErrorBody : class
    {
        await UpdateVoidAsync<TErrorBody>(uri, value, ProcessError<TErrorBody>, headers).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(
        string uri,
        object? value,
        Func<object, Exception?> processError,
        List<Header>? headers = null)
    {
        await UpdateVoidAsync<object>(uri, value, processError, headers).ConfigureAwait(false);
    }


    public virtual async Task UpdateAsync(
        string uri,
        object? value,
        List<Header>? headers = null)
    {
        await UpdateAsync(uri, value, ProcessError<object>, headers).ConfigureAwait(false);
    }
    #endregion

    private async Task<TSuccessBody> ExecuteRequest<TSuccessBody, TErrorBody>(
        Func<Task<HttpResponseMessage>> RequestAsync,
        Func<TErrorBody, Exception?> processError,
        List<Header>? headers = null)
        where TSuccessBody : class
        where TErrorBody : class
    {
        try
        {
            headers ??= [];

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
        Func<Task<HttpResponseMessage>> RequestAsync,
        Func<TErrorBody, Exception?> processError,
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
        Func<TErrorBody, Exception?> processErrorResponse)
        where TSuccessBody : class
        where TErrorBody : class
    {
        await ProcessErrorAsync(response, processErrorResponse).ConfigureAwait(false);

        var successBody = await ProcessBodyAsync<TSuccessBody>(response).ConfigureAwait(false);

        return successBody;
    }

    private async Task ProcessResponseAsync<TErrorBody>(
        HttpResponseMessage response,
        Func<TErrorBody, Exception?> processErrorResponse)
        where TErrorBody : class
    {
        await ProcessErrorAsync(response, processErrorResponse).ConfigureAwait(false);
    }

    private async Task ProcessErrorAsync<TErrorBody>(
        HttpResponseMessage response,
        Func<TErrorBody, Exception?> processErrorResponse)
        where TErrorBody : class
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var concreteErrorBody = await ProcessBodyAsync<TErrorBody>(response).ConfigureAwait(false);

        if (concreteErrorBody == null)
        {
            var errorBody = await ProcessBodyAsync<object>(response).ConfigureAwait(false);

            throw new RequestException<object>(errorBody);
        }

        var exception = processErrorResponse(concreteErrorBody);
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
        {
            throw new BodyNullException();
        }

        return body;
    }

    protected Exception ProcessError<TErrorBody>(TErrorBody errorBody)
        where TErrorBody : class
    {
        return new RequestException<TErrorBody>(errorBody);
    }
}
