using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class RepaymentSystem : ValueObject
{
    public RepaymentSystem(Refinance? refinance = null, Repay? repay = null)
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
            FundingFormOption.Refinance => new RepaymentSystem(refinance: new Refinance(value, additionalInformation!)),
            FundingFormOption.Repay => new RepaymentSystem(repay: new Repay(value)),
            _ => throw new ArgumentException("Provided invalid value"),
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
