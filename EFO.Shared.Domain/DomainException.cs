using System.Text;

namespace EFO.Shared.Domain;

public class DomainException : Exception
{
    public DomainException(params DomainError[] errors)
    {
        Errors = errors;
    }

    public DomainError[] Errors { get; }

    public static void ThrowIfErrors(IEnumerable<DomainError> errors)
    {
        var errorsArray = errors as DomainError[] ?? errors.ToArray();
        if (errorsArray.Any())
        {
            throw new DomainException(errorsArray);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("DomainException:");
        foreach (var error in Errors)
        {
            sb.AppendLine(error.Name);
        }

        return sb.ToString();
    }
}
