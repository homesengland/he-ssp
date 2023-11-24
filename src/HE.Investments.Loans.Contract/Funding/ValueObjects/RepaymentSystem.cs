using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public class RepaymentSystem : ValueObject
{
    public RepaymentSystem(Refinance? refinance, Repay? repay)
    {
        Refinance = refinance;
        Repay = repay;
    }

    public Refinance? Refinance { get; }

    public Repay? Repay { get; }

    public static RepaymentSystem New(string value, string? additionalInformation)
    {
        var result = value switch
        {
            FundingFormOption.Refinance => new RepaymentSystem(new Refinance(value, additionalInformation!), null),
            FundingFormOption.Repay => new RepaymentSystem(null, repay: new Repay(value)),
            _ => throw new DomainValidationException(OperationResult.New().AddValidationError(nameof(RepaymentSystem), ValidationErrorMessage.InvalidValue)),
        };
        return result;
    }

    public override string? ToString()
    {
        return Refinance == null ? Repay?.ToString() : Refinance.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Refinance;
        yield return Repay;
    }
}
