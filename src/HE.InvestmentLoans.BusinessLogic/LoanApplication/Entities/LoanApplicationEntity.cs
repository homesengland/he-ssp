using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, UserAccount userAccount, ApplicationStatus externalStatus)
    {
        Id = id;
        UserAccount = userAccount;
        ApplicationProjects = new ApplicationProjects(Id);
        ExternalStatus = externalStatus;
    }

    public LoanApplicationId Id { get; private set; }

    public UserAccount UserAccount { get; }

    public ApplicationProjects ApplicationProjects { get; private set; }

    public LoanApplicationViewModel LegacyModel { get; set; }

    public ApplicationStatus ExternalStatus { get; set; }

    public static LoanApplicationEntity New(UserAccount userAccount) => new(LoanApplicationId.New(), userAccount, ApplicationStatus.Draft);

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
                ("Date", LegacyModel.Timestamp.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
                ("Hour", LegacyModel.Timestamp.Date.ToString("hh:mm tt", CultureInfo.InvariantCulture)));
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
