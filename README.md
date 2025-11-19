# Poolz.Finance.CSharp.Http

Poolz.Finance.CSharp.Http is a lightweight .NET 8 library that centralizes the creation of `HttpClient` instances for Poolz Finance services.
It wraps the default `HttpClientHandler` with a custom `FailureOnlyLoggingHandler` that surfaces detailed information when requests fail while keeping successful responses silent.

## Getting started

```csharp
using Poolz.Finance.CSharp.Http;

var factory = new HttpClientFactory();
var client = factory.Create(
    url: "https://api.poolz.finance",
    configureHeaders: headers =>
    {
        headers.Add("Authorization", "Bearer <token>");
        headers.Add("User-Agent", "Poolz-SDK-Demo/1.0");
    });

var response = await client.GetAsync("/v1/pools");
var content = await response.Content.ReadAsStringAsync();
```

If the request fails, `FailureOnlyLoggingHandler` rethrows an exception that includes the HTTP method and URL so that you immediately know which call needs attention.

