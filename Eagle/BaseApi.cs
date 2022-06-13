using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Eagle
{

    public abstract class BaseApi : IDisposable
    {
        public BaseApi(string host = "localhost", int port = 41595)
        {
            _builder = new UriBuilder
            {
                Scheme = "http",
                Host = host,
                Port = port
            };
            _client = new HttpClient();
        }

        protected async Task<T?> GetAsync<T>(string path,CancellationToken token)
        {
            try
            {
                var fullUri = GetUri(path);
                return await _client.GetFromJsonAsync<T>(fullUri, token);
            }
            catch (Exception ex)
            {
                throw new EagleException("Eagle Failed to Response",ex);
            }
        }
        
        protected async Task<T?> GetAsync<T>(Uri uri,CancellationToken token)
        {
            try
            {
                var full = uri.ToString();
                return await _client.GetFromJsonAsync<T>(uri, token);
            }
            catch (Exception ex)
            {
                throw new EagleException("Eagle Failed to Response",ex);
            }
        }
        

        protected async Task<TReceive?> PostAsync<TSend, TReceive>(string path, TSend data,CancellationToken token)
        {
            try
            {
                var postResponse = await _client.PostAsJsonAsync(GetUri(path), data,token);
                postResponse.EnsureSuccessStatusCode();
                return await postResponse.Content.ReadFromJsonAsync<TReceive>();
            }
            catch (Exception ex)
            {
                throw new EagleException("Eagle Failed to Response",ex);
            }
        }

        protected Uri GetUri(string relative)
        {
            _builder.Path = $"{apiPrefix}{relative}";
            return _builder.Uri;
        }

        private UriBuilder _builder;
        private HttpClient _client;
        private string apiPrefix = "/api/";
        public void Dispose() => _client.Dispose();
    }
}