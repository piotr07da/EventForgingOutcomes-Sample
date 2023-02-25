using System.Collections.ObjectModel;

namespace EFO.Shared.Domain;

public sealed class DomainError
{
    private readonly IDictionary<string, object> _data;

    public DomainError(string name)
    {
        Name = name;
        _data = new Dictionary<string, object>();
    }

    public string Name { get; }
    public IReadOnlyDictionary<string, object> Data => new ReadOnlyDictionary<string, object>(_data);

    public DomainError WithData(string key, object value)
    {
        _data[key] = value;
        return this;
    }
}
