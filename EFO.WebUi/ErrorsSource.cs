using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EFO.WebUi;

public sealed class ErrorsSource : IErrorsSource
{
    private readonly ISubject<string> _errors = new Subject<string>();

    public IObservable<string> Errors => _errors.AsObservable();

    public void Notify(string error)
    {
        _errors.OnNext(error);
    }
}
