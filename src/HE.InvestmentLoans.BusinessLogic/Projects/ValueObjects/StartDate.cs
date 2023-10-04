using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class StartDate : ValueObject
{
    public StartDate(bool exists, DateTime? value)
    {
        Exists = exists;

        if (exists && value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(StartDate), ValidationErrorMessage.NoStartDate)
                .CheckErrors();
        }

        Value = value;
    }

    public DateTime? Value { get; }

    public bool Exists { get; }

    public static StartDate From(string existsString, string day, string month, string year)
    {
        var exists = existsString.MapToNonNullableBool();

        if (!exists)
        {
            return new StartDate(exists, null);
        }

        if (day.IsNotProvided() || month.IsNotProvided() || year.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(StartDate), ValidationErrorMessage.NoStartDate)
                .CheckErrors();
        }

        var dateString = $"{day}/{month}/{year}";

        if (!DateTime.TryParseExact(dateString, ProjectFormOption.AllowedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(StartDate), ValidationErrorMessage.InvalidStartDate)
                .CheckErrors();
        }

        return new StartDate(exists, dateValue);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
