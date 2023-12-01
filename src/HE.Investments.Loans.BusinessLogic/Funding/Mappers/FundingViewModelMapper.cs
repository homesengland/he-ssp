using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Funding.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Funding.Mappers;

public static class FundingViewModelMapper
{
    public static FundingViewModel Map(FundingEntity fundingEntity)
    {
        return new FundingViewModel
        {
            LoanApplicationId = fundingEntity.LoanApplicationId.Value,
            LoanApplicationStatus = fundingEntity.LoanApplicationStatus,
            GrossDevelopmentValue = fundingEntity.GrossDevelopmentValue?.ToString(),
            TotalCosts = fundingEntity.EstimatedTotalCosts?.ToString(),
            AbnormalCosts = MapAbnormalCosts(fundingEntity.AbnormalCosts),
            AbnormalCostsInfo = fundingEntity.AbnormalCosts?.AbnormalCostsAdditionalInformation,
            PrivateSectorFunding = MapPrivateSectorFunding(fundingEntity.PrivateSectorFunding),
            PrivateSectorFundingResult = fundingEntity.PrivateSectorFunding?.PrivateSectorFundingApplyResult,
            PrivateSectorFundingReason = fundingEntity.PrivateSectorFunding?.PrivateSectorFundingNotApplyingReason,
            Refinance = MapRepaymentSystem(fundingEntity.RepaymentSystem),
            RefinanceInfo = fundingEntity.RepaymentSystem?.Refinance?.AdditionalInformation,
            AdditionalProjects = MapAdditionalProjects(fundingEntity.AdditionalProjects),
            CheckAnswers = fundingEntity.Status == SectionStatus.Completed ? CommonResponse.Yes : null,
            State = fundingEntity.Status,
        };
    }

    public static string? MapAbnormalCosts(AbnormalCosts? abnormalCosts)
    {
        if (abnormalCosts is null)
        {
            return null;
        }

        return abnormalCosts.IsAnyAbnormalCost ? CommonResponse.Yes : CommonResponse.No;
    }

    public static string? MapPrivateSectorFunding(PrivateSectorFunding? privateSectorFunding)
    {
        if (privateSectorFunding is null)
        {
            return null;
        }

        return privateSectorFunding.IsApplied ? CommonResponse.Yes : CommonResponse.No;
    }

    public static string? MapRepaymentSystem(RepaymentSystem? repaymentSystem)
    {
        if (repaymentSystem is null)
        {
            return null;
        }

        return repaymentSystem.Refinance?.Value != null ? FundingFormOption.Refinance : FundingFormOption.Repay;
    }

    public static string? MapAdditionalProjects(AdditionalProjects? additionalProjects)
    {
        if (additionalProjects is null)
        {
            return null;
        }

        return additionalProjects.IsThereAnyAdditionalProject ? CommonResponse.Yes : CommonResponse.No;
    }
}
