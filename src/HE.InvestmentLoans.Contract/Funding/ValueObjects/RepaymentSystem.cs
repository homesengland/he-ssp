using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

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
