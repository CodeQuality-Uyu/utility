using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class ConcreteHttpClient<TGenericError> : HttpClientAdapter
        where TGenericError : class
    {
        public ConcreteHttpClient(string baseUrl) : base(baseUrl) { }

        public ConcreteHttpClient(HttpClient client) : base(client) { }

        #region Post
        public async Task<TSuccessBody> PostAsync<TSuccessBody>(
            string uri,
            object value,
            Func<TGenericError, Exception?> processError,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await base.PostAsync<TSuccessBody, TGenericError>(uri, value, processError, headers).ConfigureAwait(false);

            return response;
        }

        public async Task<TSuccessBody> PostAsync<TSuccessBody>(
            string uri,
            object value,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await this.PostAsync<TSuccessBody>(uri, value, ProcessError, headers).ConfigureAwait(false);

            return response;
        }

        public async Task PostAsync(
            string uri,
            Func<TGenericError, Exception?> processError,
            object value,
            List<Header>? headers = null)
        {
            await base.PostVoidAsync<TGenericError>(uri, value, processError, headers).ConfigureAwait(false);
        }

        public async Task PostAsync(
            string uri,
            object value,
            List<Header>? headers = null)
        {
            await this.PostVoidAsync<TGenericError>(uri, value, this.ProcessError, headers).ConfigureAwait(false);
        }
        #endregion

        #region Get
        public async Task<TSuccessBody> GetAsync<TSuccessBody>(
            string uri,
            Func<TGenericError, Exception?> processError,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await base.GetAsync<TSuccessBody, TGenericError>(uri, processError, headers).ConfigureAwait(false);

            return response;
        }

        public async Task<TSuccessBody> GetAsync<TSuccessBody>(
            string uri,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await this.GetAsync<TSuccessBody>(uri, ProcessError, headers).ConfigureAwait(false);

            return response;
        }
        #endregion

        #region Delete
        public async Task<TSuccessBody> DeleteAsync<TSuccessBody>(
           string uri,
           Func<TGenericError, Exception?> processError,
           List<Header>? headers = null)
           where TSuccessBody : class
        {
            var response = await base.DeleteAsync<TSuccessBody, TGenericError>(uri, processError, headers).ConfigureAwait(false);

            return response;
        }

        public async Task<TSuccessBody> DeleteAsync<TSuccessBody>(
           string uri,
           List<Header>? headers = null)
           where TSuccessBody : class
        {
            var response = await this.DeleteAsync<TSuccessBody>(uri, ProcessError, headers).ConfigureAwait(false);

            return response;
        }

        public async Task DeleteAsync(
            string uri,
            Func<TGenericError, Exception?> processError,
            List<Header>? headers = null)
        {
            await base.DeleteVoidAsync<TGenericError>(uri, processError, headers).ConfigureAwait(false);
        }

        public async Task DeleteAsync(
            string uri,
            List<Header>? headers = null)
        {
            await this.DeleteAsync(uri, ProcessError, headers).ConfigureAwait(false);
        }
        #endregion

        #region Update
        public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody>(
            string uri,
            object? value,
            Func<TGenericError, Exception?> processError,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await base.UpdateAsync<TSuccessBody, TGenericError>(uri, value, processError, headers).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<TSuccessBody> UpdateAsync<TSuccessBody>(
            string uri,
            object? value,
            List<Header>? headers = null)
            where TSuccessBody : class
        {
            var response = await this.UpdateAsync<TSuccessBody>(uri, value, ProcessError, headers).ConfigureAwait(false);

            return response;
        }

        public virtual async Task UpdateAsync(
            string uri,
            object? value,
            Func<TGenericError, Exception?> processError,
            List<Header>? headers = null)
        {
            await base.UpdateVoidAsync<TGenericError>(uri, value, processError, headers).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(
            string uri,
            object? value,
            List<Header>? headers = null)
        {
            await base.UpdateVoidAsync<TGenericError>(uri, value, ProcessError, headers).ConfigureAwait(false);
        }
        #endregion

        protected virtual Exception? ProcessError(TGenericError error)
        {
            return base.ProcessError<TGenericError>(error);
        }
    }
}
