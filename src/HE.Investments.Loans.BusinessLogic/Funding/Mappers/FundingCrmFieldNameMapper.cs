using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.Common.Utils.Enums;

namespace HE.Investments.Loans.BusinessLogic.Funding.Mappers;

public static class FundingCrmFieldNameMapper
{
    private static readonly string ExternalStatus = $"{nameof(invln_Loanapplication.invln_ExternalStatus).ToLowerInvariant()},";
    private static readonly string ProjectGdv = $"{nameof(invln_Loanapplication.invln_ProjectGDV).ToLowerInvariant()},";
    private static readonly string ProjectEstimatedTotalCost = $"{nameof(invln_Loanapplication.invln_Projectestimatedtotalcost).ToLowerInvariant()},";
    private static readonly string ProjectAbnormalCosts = $"{nameof(invln_Loanapplication.invln_Projectabnormalcosts).ToLowerInvariant()},";

    private static readonly string ProjectAbnormalCostsInformation =
        $"{nameof(invln_Loanapplication.invln_Projectabnormalcostsinformation).ToLowerInvariant()},";

    private static readonly string PrivateSectorApproach = $"{nameof(invln_Loanapplication.invln_Privatesectorapproach).ToLowerInvariant()},";

    private static readonly string PrivateSectorApproachInformation =
        $"{nameof(invln_Loanapplication.invln_Privatesectorapproachinformation).ToLowerInvariant()},";

    private static readonly string AdditionalProjects = $"{nameof(invln_Loanapplication.invln_Additionalprojects).ToLowerInvariant()},";
    private static readonly string RefinanceRepayment = $"{nameof(invln_Loanapplication.invln_Refinancerepayment).ToLowerInvariant()},";
    private static readonly string RefinanceRepaymentDetails = $"{nameof(invln_Loanapplication.invln_Refinancerepaymentdetails).ToLowerInvariant()},";
    private static readonly string FundingDetailsCompletionStatus = $"{nameof(invln_Loanapplication.invln_fundingdetailscompletionstatus).ToLowerInvariant()}";

    public static string Map(FundingFieldsSet fundingFieldsSet)
    {
        var result = fundingFieldsSet switch
        {
            FundingFieldsSet.GetEmpty => ExternalStatus,
            FundingFieldsSet.GDV => ProjectGdv,
            FundingFieldsSet.EstimatedTotalCosts => ProjectEstimatedTotalCost,
            FundingFieldsSet.PrivateSectorFunding => PrivateSectorApproach + PrivateSectorApproachInformation,
            FundingFieldsSet.AbnormalCosts => ProjectAbnormalCosts + ProjectAbnormalCostsInformation,
            FundingFieldsSet.RepaymentSystem => RefinanceRepayment + RefinanceRepaymentDetails,
            FundingFieldsSet.AdditionalProjects => AdditionalProjects,
            FundingFieldsSet.GetAllFields => ExternalStatus +
                                             ProjectGdv +
                                             ProjectEstimatedTotalCost +
                                             ProjectAbnormalCosts +
                                             ProjectAbnormalCostsInformation +
                                             PrivateSectorApproach +
                                             PrivateSectorApproachInformation +
                                             AdditionalProjects +
                                             RefinanceRepayment +
                                             RefinanceRepaymentDetails,
            FundingFieldsSet.SaveAllFields => ProjectGdv +
                                              ProjectEstimatedTotalCost +
                                              ProjectAbnormalCosts +
                                              ProjectAbnormalCostsInformation +
                                              PrivateSectorApproach +
                                              PrivateSectorApproachInformation +
                                              AdditionalProjects +
                                              RefinanceRepayment +
                                              RefinanceRepaymentDetails,
            _ => string.Empty,
        };

        return result + FundingDetailsCompletionStatus;
    }
}
