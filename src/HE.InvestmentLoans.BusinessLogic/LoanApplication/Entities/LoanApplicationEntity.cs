using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, UserAccount userAccount, ApplicationStatus externalStatus, FundingPurpose fundingReason, DateTime? createdOn, DateTime? lastModificationDate)
    {
        Id = id;
        UserAccount = userAccount;
        ApplicationProjects = new ApplicationProjects(Id);
        ExternalStatus = externalStatus;
        LastModificationDate = lastModificationDate;
        FundingReason = fundingReason;
        CreatedOn = createdOn;
    }

    public LoanApplicationId Id { get; private set; }

    public UserAccount UserAccount { get; }

    public ApplicationProjects ApplicationProjects { get; private set; }

    public LoanApplicationViewModel LegacyModel { get; set; }

    public ApplicationStatus ExternalStatus { get; set; }

    public DateTime? LastModificationDate { get; }

    public DateTime? CreatedOn { get; }

    public FundingPurpose FundingReason { get; private set; }

    public string ReferenceNumber => LegacyModel.ReferenceNumber ?? string.Empty;

    // TODO: #77804
    public string Name => ReferenceNumber;

    public static LoanApplicationEntity New(UserAccount userAccount) => new(LoanApplicationId.New(), userAccount, ApplicationStatus.Draft, FundingPurpose.BuildingNewHomes, null, null);

    public void SaveApplicationProjects(ApplicationProjects applicationProjects)
    {
        ApplicationProjects = applicationProjects;
    }

    public void SetId(LoanApplicationId newId)
    {
        if (Id.IsSaved())
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
        SyncToLegacyModel();
    }

    public async Task Submit(ICanSubmitLoanApplication canSubmitLoanApplication, UserAccount userAccount, CancellationToken cancellationToken)
    {
        CheckIfCanBeSubmitted();

        await canSubmitLoanApplication.Submit(Id, cancellationToken);
    }

    public void CheckIfCanBeSubmitted()
    {
        if (!IsReadyToSubmit())
        {
            throw new DomainException("Loan application is not ready to be submitted", CommonErrorCodes.LoanApplicationNotReadyToSubmit);
        }

        if (IsSubmitted())
        {
            throw new DomainException(
                "Loan application has been submitted",
                CommonErrorCodes.ApplicationHasBeenSubmitted,
                ("Date", LastModificationDate!.Value));
        }
    }

    public async Task Withdraw(ILoanApplicationRepository loanApplicationRepository, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        var statusesAfterSubmit = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();

        if (ExternalStatus == ApplicationStatus.Draft)
        {
            await loanApplicationRepository.WithdrawDraft(Id, withdrawReason, cancellationToken);
        }
        else if (statusesAfterSubmit.Contains(ExternalStatus))
        {
            await loanApplicationRepository.WithdrawSubmitted(Id, withdrawReason, cancellationToken);
        }
        else
        {
            throw new DomainException("Loan application cannot be withdrawn", CommonErrorCodes.LoanApplicationCannotBeWithdrawn);
        }
    }

    public bool IsEnoughHomesToBuild()
    {
        const int minimumHomesToBuild = 5;
        var cultureInfo = CultureInfo.InvariantCulture;
        var result = LegacyModel.Sites
                        .Select(site => site.ManyHomes)
                        .Where(manyHomes => !string.IsNullOrEmpty(manyHomes))
                        .Select(manyHomes => int.TryParse(manyHomes, NumberStyles.Integer, cultureInfo, out var parsedValue) ? parsedValue : 0)
                        .Aggregate(0, (x, y) => x + y);

        return result >= minimumHomesToBuild;
    }

    private bool IsReadyToSubmit()
    {
        return LegacyModel.IsReadyToSubmit();
    }

    private bool IsSubmitted()
    {
        return ExternalStatus == ApplicationStatus.ApplicationSubmitted;
    }

    private void SyncToLegacyModel()
    {
        LegacyModel = new LoanApplicationViewModel
        {
            ID = Id.Value,
            State = LoanApplicationWorkflow.State.TaskList,
        };
        LegacyModel.AddNewSite();
    }
}
