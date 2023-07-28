using Dawn;

namespace HE.InvestmentLoans.Common.Utils.ValueObjects;

public class ControllerName
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
}
