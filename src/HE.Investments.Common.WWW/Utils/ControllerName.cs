using Dawn;
using HE.InvestmentLoans.Common.Domain;

namespace HE.Investments.Common.WWW.Utils;

public class ControllerName : ValueObject
{
    private readonly string _name;

    public ControllerName(string controllerFullName)
    {
        _name = Guard.Argument(controllerFullName, nameof(controllerFullName)).NotNull().NotEmpty();
    }

    public string WithoutPrefix()
    {
        return _name.Replace("Controller", string.Empty, StringComparison.CurrentCulture);
    }

    public override bool Equals(object? obj)
    {
        if (obj is string s)
        {
            return _name.Equals(s, StringComparison.Ordinal);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _name;
    }
}
