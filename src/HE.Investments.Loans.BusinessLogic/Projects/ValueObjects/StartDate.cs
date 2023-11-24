using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;
using HE.Investments.Loans.Common.Extensions;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class StartDate : ValueObject
{
    public StartDate(bool exists, ProjectDate? value)
    {
        Exists = exists;

        if (exists && value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(ProjectValidationFieldNames.StartDate, ValidationErrorMessage.NoStartDate)
                .CheckErrors();
        }

        Date = value;
    }

    public ProjectDate? Date { get; }

    public DateTime? Value => Date?.Value;

    public bool Exists { get; }

    public static StartDate From(string existsString, string year, string month, string day)
    {
        var exists = existsString.MapToNonNullableBool();

        if (!exists)
        {
            return new StartDate(exists, null);
        }

        var operationResult = OperationResult.ResultOf(() => ProjectDate.FromString(year, month, day));

        operationResult.OverrideError(GenericValidationError.NoDate, ProjectValidationFieldNames.StartDay, ValidationErrorMessage.NoStartDate);
        operationResult.OverrideError(GenericValidationError.InvalidDate, ProjectValidationFieldNames.StartDay, ValidationErrorMessage.InvalidStartDate);

        operationResult.CheckErrors();

        return new StartDate(exists, operationResult.ReturnedData);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
        yield return Exists;
    }
}
