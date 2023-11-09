using System.Globalization;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class ProjectDate : ValueObject
{
    public ProjectDate(DateTime value)
    {
        Value = value;
    }

    public DateTime Value { get; }

    public static ProjectDate FromString(string year, string month, string day)
    {
        var operationResult = OperationResult.New();

        if (day.IsNotProvided() || month.IsNotProvided() || year.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(ProjectDate), GenericValidationError.NoDate)
                .CheckErrors();
        }

        var dateString = $"{day}/{month}/{year}";

        if (!DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(ProjectDate), GenericValidationError.InvalidDate)
                .CheckErrors();
        }

        return new ProjectDate(dateValue);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
