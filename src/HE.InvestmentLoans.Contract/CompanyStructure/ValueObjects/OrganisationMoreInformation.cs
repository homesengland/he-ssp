using Dawn;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformation : ValueObject
{
    public OrganisationMoreInformation(string information)
    {
        information = information.Trim();
        if (information.Length > MaximumInputLength.LongInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(OrganisationMoreInformation), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.OrganisationMoreInformation))
                .CheckErrors();
        }

        Information = Guard.Argument(information, nameof(Information)).NotEmpty();
    }

    public string Information { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Information;
    }
}
