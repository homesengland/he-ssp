using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.WWW.Utils.ValueObjects;

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

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _name;
    }
}
