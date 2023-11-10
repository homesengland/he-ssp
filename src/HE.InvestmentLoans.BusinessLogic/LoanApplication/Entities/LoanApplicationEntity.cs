using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity : DomainEntity
{
    public LoanApplicationEntity(
        LoanApplicationId id,
        LoanApplicationName name,
        UserAccount userAccount,
        ApplicationStatus externalStatus,
        FundingPurpose fundingReason,
        DateTime? createdOn,
        DateTime? lastModificationDate,
        DateTime? submittedOn,
        string lastModifiedBy,
        LoanApplicationSection companyStructure,
        LoanApplicationSection security,
        LoanApplicationSection funding,
        ProjectsSection projectsSection,
        string referenceNumber)
    {
        Id = id;
        Name = name;
        UserAccount = userAccount;
        ExternalStatus = externalStatus;
        LastModificationDate = lastModificationDate;
        SubmittedOn = submittedOn;
        LastModifiedBy = lastModifiedBy;
        FundingReason = fundingReason;
        CreatedOn = createdOn;
        CompanyStructure = companyStructure;
        Security = security;
        Funding = funding;
        ProjectsSection = projectsSection;
        ReferenceNumber = referenceNumber;
    }

    public LoanApplicationId Id { get; private set; }

    public LoanApplicationName Name { get; }

    public UserAccount UserAccount { get; }

    public LoanApplicationSection CompanyStructure { get; private set; }

    public LoanApplicationSection Security { get; private set; }

    public LoanApplicationSection Funding { get; private set; }

    public ProjectsSection ProjectsSection { get; private set; }

    public ApplicationStatus ExternalStatus { get; set; }

    public DateTime? LastModificationDate { get; }

    public string LastModifiedBy { get; }

    public DateTime? CreatedOn { get; }

    public DateTime? SubmittedOn { get; }

    public FundingPurpose FundingReason { get; private set; }

    public string ReferenceNumber { get; private set; }

    public static LoanApplicationEntity New(UserAccount userAccount, LoanApplicationName name) => new(LoanApplicationId.New(), name, userAccount, ApplicationStatus.Draft, FundingPurpose.BuildingNewHomes, null, null, null, string.Empty, LoanApplicationSection.New(), LoanApplicationSection.New(), LoanApplicationSection.New(), ProjectsSection.Empty(), string.Empty);

    public void SetId(LoanApplicationId newId)
    {
        if (Id.IsSaved())
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(ExternalStatus);
    }

    public async Task Submit(ICanSubmitLoanApplication canSubmitLoanApplication, CancellationToken cancellationToken)
    {
        CheckIfCanBeSubmitted();

        await canSubmitLoanApplication.Submit(Id, cancellationToken);
    }

    public bool CanBeSubmitted()
    {
        return IsReadyToSubmit() && !IsSubmitted();
    }

    public bool WasSubmitted() => SubmittedOn != null;

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
        var statusesAfterSubmit = ApplicationStatusDivision.GetAllStatusesAfterSubmit();

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

        Publish(new LoanApplicationHasBeenWithdrawnEvent(Id, Name));
    }

    public bool IsEnoughHomesToBuild()
    {
        const int minimumHomesToBuild = 5;

        return ProjectsSection.TotalHomesBuilt() >= minimumHomesToBuild;
    }

    private bool IsReadyToSubmit()
    {
        return ProjectsSection.IsCompleted() && Funding.IsCompleted() && Security.IsCompleted() && CompanyStructure.IsCompleted();
    }

    private bool IsSubmitted()
    {
        return ExternalStatus == ApplicationStatus.ApplicationSubmitted;
    }
}
