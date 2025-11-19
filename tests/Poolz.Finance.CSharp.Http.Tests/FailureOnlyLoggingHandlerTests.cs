using Xunit;
using System.Net;

namespace Poolz.Finance.CSharp.Http.Tests;

public class FailureOnlyLoggingHandlerTests
{
    [Fact]
    public async Task SendAsync_ReturnsResponse_WhenStatusIsSuccessful()
    {
        using var handler = new FailureOnlyLoggingHandler(new StubHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))));
        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://poolz.finance/success");

        var response = await invoker.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SendAsync_ThrowsHttpRequestExceptionWithContext_WhenStatusIsFailure()
    {
        using var handler = new FailureOnlyLoggingHandler(new StubHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest))));
        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://poolz.finance/failure");

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => invoker.SendAsync(request, CancellationToken.None));

        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.NotNull(exception.InnerException);
        Assert.Contains("METHOD: POST", exception.Message);
        Assert.Contains("URL: https://poolz.finance/failure", exception.Message);
    }

    private sealed class StubHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return responseFactory(request, cancellationToken);
        }
    }
}