using System.Text.Json;

namespace EFO.WebUi;

public class ErrorHandlingHttpMessageHandler : DelegatingHandler
{
    private readonly IErrorsSource _errorsSource;

    public ErrorHandlingHttpMessageHandler(IErrorsSource errorsSource)
    {
        _errorsSource = errorsSource ?? throw new ArgumentNullException(nameof(errorsSource));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var errorContent = JsonSerializer.Deserialize<ErrorContent>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
            if (errorContent != null)
            {
                _errorsSource.Notify(errorContent.Message);
            }
        }

        return response;
    }

    private sealed record ErrorContent(string Message, string Exception);
}
