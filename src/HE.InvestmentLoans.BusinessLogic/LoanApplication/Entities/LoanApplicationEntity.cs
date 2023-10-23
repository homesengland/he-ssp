using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using StackExchange.Redis;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity : DomainEntity
{
    public LoanApplicationEntity(LoanApplicationId id, LoanApplicationName name, UserAccount userAccount, ApplicationStatus externalStatus,
        FundingPurpose fundingReason, DateTime? createdOn, DateTime? lastModificationDate, string lastModifiedBy, LoanApplicationSection companyStructure)
    {
        Id = id;
        Name = name;
        UserAccount = userAccount;
        ApplicationProjects = new ApplicationProjects(Id);
        ExternalStatus = externalStatus;
        LastModificationDate = lastModificationDate;
        LastModifiedBy = lastModifiedBy;
        FundingReason = fundingReason;
        CreatedOn = createdOn;
        CompanyStructure = companyStructure;
    }

    public LoanApplicationId Id { get; private set; }

    public LoanApplicationName Name { get; private set; }

    public UserAccount UserAccount { get; }

    public ApplicationProjects ApplicationProjects { get; private set; }

    public LoanApplicationViewModel LegacyModel { get; set; }

    public LoanApplicationSection CompanyStructure { get; }

    public ApplicationStatus ExternalStatus { get; set; }

    public DateTime? LastModificationDate { get; }

    public string LastModifiedBy { get; }

    public DateTime? CreatedOn { get; }

    public FundingPurpose FundingReason { get; private set; }

    public string ReferenceNumber => LegacyModel.ReferenceNumber ?? string.Empty;

    public static LoanApplicationEntity New(UserAccount userAccount, LoanApplicationName name) => new(
            LoanApplicationId.New(),
            name,
            userAccount,
            ApplicationStatus.Draft,
            FundingPurpose.BuildingNewHomes,
            null,
            null,
            string.Empty,
            LoanApplicationSection.New());

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
        var cultureInfo = CultureInfo.InvariantCulture;
        var result = LegacyModel.Projects
            .Select(site => site.HomesCount)
            .Where(manyHomes => !string.IsNullOrEmpty(manyHomes))
            .Select(manyHomes => int.TryParse(manyHomes, NumberStyles.Integer, cultureInfo, out var parsedValue) ? parsedValue : 0)
            .Aggregate(0, (x, y) => x + y);

        return result >= minimumHomesToBuild;
    }

    private bool IsReadyToSubmit()
    {
        return CompanyStructure.IsCompleted() && LegacyModel.IsReadyToSubmit();
    }

    private bool IsSubmitted()
    {
        return ExternalStatus == ApplicationStatus.ApplicationSubmitted;
    }
}
