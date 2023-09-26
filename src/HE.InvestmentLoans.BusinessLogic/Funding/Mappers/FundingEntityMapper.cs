using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Funding.Mappers;

public static class FundingEntityMapper
{
    public static GrossDevelopmentValue? MapGrossDevelopmentValue(decimal? grossDevelopmentValue)
    {
        if (grossDevelopmentValue is null)
        {
            return null;
        }

        return GrossDevelopmentValue.New(grossDevelopmentValue.Value);
    }

    public static decimal? MapGrossDevelopmentValue(GrossDevelopmentValue? grossDevelopmentValue)
    {
        if (grossDevelopmentValue is null)
        {
            return null;
        }

        return grossDevelopmentValue.Value;
    }

    public static EstimatedTotalCosts? MapEstimatedTotalCosts(decimal? estimatedTotalCosts)
    {
        if (estimatedTotalCosts is null)
        {
            return null;
        }

        return EstimatedTotalCosts.New(estimatedTotalCosts.Value);
    }

    public static decimal? MapEstimatedTotalCosts(EstimatedTotalCosts? estimatedTotalCosts)
    {
        if (estimatedTotalCosts is null)
        {
            return null;
        }

        return estimatedTotalCosts.Value;
    }

    public static AbnormalCosts? MapAbnormalCosts(bool? abnormalCosts, string? abnormalCostsInfo)
    {
        if (!abnormalCosts.HasValue)
        {
            return null;
        }

        return AbnormalCosts.New(abnormalCosts.Value, abnormalCostsInfo);
    }

    public static PrivateSectorFunding? MapPrivateSectorFunding(bool? privateSectorFunding, string? privateSectorFundingAdditionalInformation)
    {
        if (!privateSectorFunding.HasValue || string.IsNullOrWhiteSpace(privateSectorFundingAdditionalInformation))
        {
            return null;
        }

        var resultPrivateSectorFunding = privateSectorFunding.Value
            ? PrivateSectorFunding.New(privateSectorFunding.Value, privateSectorFundingAdditionalInformation, null)
            : PrivateSectorFunding.New(privateSectorFunding.Value, null, privateSectorFundingAdditionalInformation);

        return resultPrivateSectorFunding;
    }

    public static string? MapPrivateSectorFundingAdditionalInformation(PrivateSectorFunding? privateSectorFunding)
    {
        if (privateSectorFunding is null)
        {
            return null;
        }

        return privateSectorFunding.IsApplied ? privateSectorFunding.PrivateSectorFundingApplyResult : privateSectorFunding.PrivateSectorFundingNotApplyingReason;
    }

    public static RepaymentSystem? MapRepaymentSystem(string? repaymentSystem, string? refinanceAdditionalInformation)
    {
        if (string.IsNullOrWhiteSpace(repaymentSystem))
        {
            return null;
        }

        return RepaymentSystem.New(repaymentSystem, refinanceAdditionalInformation);
    }

    public static string? MapRepaymentSystem(RepaymentSystem? repaymentSystem)
    {
        if (repaymentSystem is null)
        {
            return null;
        }

        return repaymentSystem.Refinance?.Value is not null ? FundingFormOption.Refinance : FundingFormOption.Repay;
    }

    public static AdditionalProjects? MapAdditionalProjects(bool? additionalProjects)
    {
        if (!additionalProjects.HasValue)
        {
            return null;
        }

        return AdditionalProjects.New(additionalProjects.Value);
    }
}
