using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.Contract.Domain;

public abstract class ValueObjectWithErrorItem : ValueObject
{
    public ErrorItem Error { get; private set; }

    public void AddValidationError(string affectedField, string validationMessage)
    {
        Error = new ErrorItem(affectedField, validationMessage);
    }
}
