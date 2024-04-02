using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class StartDate : DateValueObject
{
    private const string FieldDescription = "you plan to start the project";

    public StartDate(bool exists, string? day, string? month, string? year, string fieldName = nameof(StartDate))
        : base(day, month, year, nameof(StartDate), FieldDescription, !exists)
    {
        Exists = exists;
    }

    public ProjectDate? Date { get; }

    public new DateTime? Value => Exists ? base.Value : null;

    public bool Exists { get; }

    public static StartDate FromProjectDate(bool exists, ProjectDate? value)
        => new(
            exists,
            value?.Value.Day.ToString(CultureInfo.InvariantCulture),
            value?.Value.Month.ToString(CultureInfo.InvariantCulture),
            value?.Value.Year.ToString(CultureInfo.InvariantCulture));

    public static StartDate From(string existsString, string year, string month, string day)
    {
        var exists = existsString.MapToNonNullableBool();

        if (!exists)
        {
            return StartDate.FromProjectDate(exists, null);
        }

        var operationResult = OperationResult.ResultOf(() => ProjectDate.FromString(year, month, day));

        operationResult.OverrideError(GenericValidationError.NoDate, ProjectValidationFieldNames.StartDay, ValidationErrorMessage.NoStartDate);
        operationResult.OverrideError(GenericValidationError.InvalidDate, ProjectValidationFieldNames.StartDay, ValidationErrorMessage.InvalidStartDate);

        operationResult.CheckErrors();

        return StartDate.FromProjectDate(exists, operationResult.ReturnedData);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
