using Dawn;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

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
