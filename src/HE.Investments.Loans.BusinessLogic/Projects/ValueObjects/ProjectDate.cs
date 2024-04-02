using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;

public class ProjectDate : ValueObject
{
    private static readonly DateTime MinDateValue = new(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

    public ProjectDate(DateTime value)
    {
        Value = value;
    }

    public DateTime Value { get; }

    public static ProjectDate FromString(string year, string month, string day)
    {
        if (day.IsNotProvided() || month.IsNotProvided() || year.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(ProjectDate), GenericValidationError.NoDate)
                .CheckErrors();
        }

        var dateString = $"{day}/{month}/{year}";

        if (!DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue)
            || dateValue < MinDateValue)
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
