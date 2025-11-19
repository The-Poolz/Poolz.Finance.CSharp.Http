using System.Net.Http.Headers;

namespace Poolz.Finance.CSharp.Http;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient Create(string url, Action<HttpRequestHeaders>? configureHeaders = null)
    {
        var client = new HttpClient(new FailureOnlyLoggingHandler(new HttpClientHandler()))
        {
            BaseAddress = new Uri(url)
        };

        configureHeaders?.Invoke(client.DefaultRequestHeaders);
        return client;
    }
}