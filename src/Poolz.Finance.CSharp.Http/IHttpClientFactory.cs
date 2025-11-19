using System.Net.Http.Headers;

namespace Poolz.Finance.CSharp.Http;

public interface IHttpClientFactory
{
    public HttpClient Create(string url, Action<HttpRequestHeaders>? configure = null);
}