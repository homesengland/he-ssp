using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

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

        if (string.IsNullOrEmpty(information))
        {
            throw new ArgumentNullException(nameof(information));
        }

        Information = information;
    }

    public string Information { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Information;
    }
}
