using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class ProjectName : ValueObject
{
    public ProjectName(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(ProjectName), ValidationErrorMessage.ProjectNameIsEmpty)
                .CheckErrors();
        }

        if (value.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(ProjectName), ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.ProjectName))
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
