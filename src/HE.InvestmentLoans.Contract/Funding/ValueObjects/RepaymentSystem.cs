using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
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
