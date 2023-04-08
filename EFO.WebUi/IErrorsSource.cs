namespace EFO.WebUi;

public interface IErrorsSource
{
    IObservable<string> Errors { get; }
    public void Notify(string error);
}
