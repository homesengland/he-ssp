using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Funding.Entities;
public class FundingEntity
{
    public FundingEntity(
        LoanApplicationId loanApplicationId,
        GrossDevelopmentValue? grossDevelopmentValue,
        EstimatedTotalCosts? estimatedTotalCosts,
        AbnormalCosts? abnormalCosts,
        PrivateSectorFunding? privateSectorFunding,
        RepaymentSystem? repaymentSystem,
        AdditionalProjects? additionalProjects,
        SectionStatus status)
    {
        LoanApplicationId = loanApplicationId;
        GrossDevelopmentValue = grossDevelopmentValue;
        EstimatedTotalCosts = estimatedTotalCosts;
        AbnormalCosts = abnormalCosts;
        PrivateSectorFunding = privateSectorFunding;
        RepaymentSystem = repaymentSystem;
        AdditionalProjects = additionalProjects;
        Status = status;
    }

    public GrossDevelopmentValue? GrossDevelopmentValue { get; private set; }

    public EstimatedTotalCosts? EstimatedTotalCosts { get; private set; }

    public AbnormalCosts? AbnormalCosts { get; private set; }

    public PrivateSectorFunding? PrivateSectorFunding { get; private set; }

    public RepaymentSystem? RepaymentSystem { get; private set; }

    public AdditionalProjects? AdditionalProjects { get; private set; }

    public SectionStatus Status { get; private set; }

    public LoanApplicationId LoanApplicationId { get; }

    public void ProvideGrossDevelopmentValue(GrossDevelopmentValue? grossDevelopmentValue)
    {
        if (GrossDevelopmentValue == grossDevelopmentValue)
        {
            return;
        }

        GrossDevelopmentValue = grossDevelopmentValue;
        UnCompleteSection();
    }

    public void ProvideEstimatedTotalCosts(EstimatedTotalCosts? estimatedTotalCosts)
    {
        if (EstimatedTotalCosts == estimatedTotalCosts)
        {
            return;
        }

        EstimatedTotalCosts = estimatedTotalCosts;
        UnCompleteSection();
    }

    public void ProvideAbnormalCosts(AbnormalCosts? abnormalCosts)
    {
        if (AbnormalCosts == abnormalCosts)
        {
            return;
        }

        AbnormalCosts = abnormalCosts;
        UnCompleteSection();
    }

    public void ProvidePrivateSectorFunding(PrivateSectorFunding? privateSectorFunding)
    {
        if (PrivateSectorFunding == privateSectorFunding)
        {
            return;
        }

        PrivateSectorFunding = privateSectorFunding;
        UnCompleteSection();
    }

    public void ProvideRepaymentSystem(RepaymentSystem? repaymentSystem)
    {
        if (RepaymentSystem == repaymentSystem)
        {
            return;
        }

        RepaymentSystem = repaymentSystem;
        UnCompleteSection();
    }

    public void ProvideAdditionalProjects(AdditionalProjects? additionalProjects)
    {
        if (AdditionalProjects == additionalProjects)
        {
            return;
        }

        AdditionalProjects = additionalProjects;
        UnCompleteSection();
    }

    public void CheckAnswers(YesNoAnswers yesNoAnswers)
    {
        switch (yesNoAnswers)
        {
            case YesNoAnswers.Yes:
                CompleteSection();
                break;
            case YesNoAnswers.No:
                UnCompleteSection();
                break;
            case YesNoAnswers.Undefined:
                OperationResult.New()
                    .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.SecurityCheckAnswers)
                    .CheckErrors();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(yesNoAnswers), yesNoAnswers, null);
        }
    }

    private void CompleteSection()
    {
        if (GrossDevelopmentValue.IsNotProvided() ||
            EstimatedTotalCosts.IsNotProvided() ||
            AbnormalCosts.IsNotProvided() ||
            PrivateSectorFunding.IsNotProvided() ||
            RepaymentSystem.IsNotProvided() ||
            AdditionalProjects.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.CheckAnswersOption)
                .CheckErrors();
        }

        Status = SectionStatus.Completed;
    }

    private void UnCompleteSection()
    {
        Status = SectionStatus.InProgress;
    }
}
