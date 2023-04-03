using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;

namespace EFO.WebUi;

public class ErrorHandlingHttpMessageHandler : DelegatingHandler
{
    private readonly IToastService _toastService;
    private readonly IStateHasChangedSource _stateHasChangedSource;

    public ErrorHandlingHttpMessageHandler(IToastService toastService, IStateHasChangedSource stateHasChangedSource)
    {
        _toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        _stateHasChangedSource = stateHasChangedSource ?? throw new ArgumentNullException(nameof(stateHasChangedSource));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;

        response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            try
            {
                //_toastService.ShowError(response.StatusCode.ToString());
                _toastService.ShowSuccess("Operacja zakończona sukcesem.", s => s.Position = ToastPosition.TopCenter);
                _stateHasChangedSource.Notify();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
        }

        return response;
    }
}

public interface IStateHasChangedSource
{
    IObservable<object> ChangeCall { get; }
    public void Notify();
}

public sealed class StateHasChangedSource : IStateHasChangedSource
{
    private readonly ISubject<object> _changeCall = new Subject<object>();

    public IObservable<object> ChangeCall => _changeCall.AsObservable();

    public void Notify()
    {
        _changeCall.OnNext(new object());
    }
}
