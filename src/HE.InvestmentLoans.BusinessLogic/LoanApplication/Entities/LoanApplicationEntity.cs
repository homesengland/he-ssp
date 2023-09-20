using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, UserAccount userAccount, ApplicationStatus externalStatus, DateTime? lastModificationDate, FundingPurpose fundingReason)
    {
        Id = id;
        UserAccount = userAccount;
        ApplicationProjects = new ApplicationProjects(Id);
        ExternalStatus = externalStatus;
        LastModificationDate = lastModificationDate;
        FundingReason = fundingReason;
    }

    public LoanApplicationId Id { get; private set; }

    public UserAccount UserAccount { get; }

    public ApplicationProjects ApplicationProjects { get; private set; }

    public LoanApplicationViewModel LegacyModel { get; set; }

    public ApplicationStatus ExternalStatus { get; set; }

    public DateTime? LastModificationDate { get; private set; }

    public FundingPurpose FundingReason { get; private set; }

    public string ReferenceNumber => LegacyModel.ReferenceNumber ?? string.Empty;

    // TODO: #77804
    public string Name => ReferenceNumber;

    public static LoanApplicationEntity New(UserAccount userAccount) => new(LoanApplicationId.New(), userAccount, ApplicationStatus.Draft, null, FundingPurpose.BuildingNewHomes);

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

    public async Task Submit(ICanSubmitLoanApplication canSubmitLoanApplication, CancellationToken cancellationToken)
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

        await canSubmitLoanApplication.Submit(Id, cancellationToken);
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
        return ExternalStatus == ApplicationStatus.Submitted;
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
