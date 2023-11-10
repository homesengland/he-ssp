using HE.InvestmentLoans.BusinessLogic.Projects.Consts;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class ProjectName : ValueObject
{
    public ProjectName(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.ProjectName, ValidationErrorMessage.ProjectNameIsEmpty)
                .CheckErrors();
        }

        if (value.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.ProjectName, ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.ProjectName))
                .CheckErrors();
        }

        Value = value;
    }

    public static ProjectName Default => new("New project");

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
