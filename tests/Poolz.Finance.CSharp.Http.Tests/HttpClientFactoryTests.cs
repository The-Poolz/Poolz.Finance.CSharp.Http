using Xunit;

namespace Poolz.Finance.CSharp.Http.Tests;

public class HttpClientFactoryTests
{
    [Fact]
    public void Create_ConfiguresBaseAddressAndHeaders()
    {
        var factory = new HttpClientFactory();
        var expectedBaseAddress = new Uri("https://api.poolz.finance/");

        using var client = factory.Create(expectedBaseAddress.ToString(), headers =>
        {
            headers.Add("X-Test", "42");
        });

        Assert.Equal(expectedBaseAddress, client.BaseAddress);
        Assert.True(client.DefaultRequestHeaders.Contains("X-Test"));
        Assert.Equal("42", client.DefaultRequestHeaders.GetValues("X-Test").Single());
    }
}