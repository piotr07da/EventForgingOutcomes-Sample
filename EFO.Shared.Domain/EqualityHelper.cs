using System.Collections;

namespace EFO.Shared.Domain;

public static class EqualityHelper
{
    public static int GetHashCode(params object[][] arraysOfComponents)
    {
        var components = new List<object>();
        foreach (var componentsOfArray in arraysOfComponents)
        {
            components.AddRange(componentsOfArray);
        }

        return GetHashCode(components);
    }

    public static int GetHashCode(params object[] components)
    {
        var hashCode = components[0].GetHashCode();
        for (var i = 1; i < components.Length; ++i)
        {
            hashCode ^= components[i].GetHashCode();
        }

        return hashCode;
    }

    public static bool Equals<T>(T lhs, T rhs, Func<T, object[][]> arraysOfComponentsExtractor)
    {
        object[] ComponentsExtractor(T x)
        {
            var arraysOfComponents = arraysOfComponentsExtractor(x);
            var components = new List<object>();
            foreach (var componentsOfArray in arraysOfComponents)
            {
                components.AddRange(componentsOfArray);
            }

            return components.ToArray();
        }

        return Equals(lhs, rhs, ComponentsExtractor);
    }

    public static bool Equals<T>(T lhs, T rhs, Func<T, object[]> componentsExtractor)
    {
        if (ReferenceEquals(lhs, rhs))
        {
            return true;
        }

        if (lhs == null || rhs == null)
        {
            return false;
        }

        var lhsComponents = componentsExtractor(lhs);
        var rhsComponents = componentsExtractor(rhs);

        for (var i = 0; i < lhsComponents.Length; ++i)
        {
            var lhsComponent = lhsComponents[i];
            var rhsComponent = rhsComponents[i];

            if (ReferenceEquals(lhsComponent, rhsComponent))
            {
                continue;
            }

            if (EnumerableEquals(lhsComponent, rhsComponent))
            {
                continue;
            }

            if (lhsComponent != null && !lhsComponent.Equals(rhsComponent))
            {
                return false;
            }
        }

        return true;
    }

    private static bool EnumerableEquals(object lhs, object rhs)
    {
        if (lhs == null || rhs == null)
        {
            return false;
        }

        if (lhs is IEnumerable lhsEnumerable && rhs is IEnumerable rhsEnumerable)
        {
            var lhsArray = lhsEnumerable.Cast<object>().ToArray();
            var rhsArray = rhsEnumerable.Cast<object>().ToArray();

            if (lhsArray.Length != rhsArray.Length)
            {
                return false;
            }

            for (var i = 0; i < lhsArray.Length; ++i)
            {
                if (!lhsArray[i].Equals(rhsArray[i]))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
