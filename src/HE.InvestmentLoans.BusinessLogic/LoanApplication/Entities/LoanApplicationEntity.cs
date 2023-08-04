using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Exceptions;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, UserAccount userAccount)
    {
        Id = id;
        UserAccount = userAccount;
        ApplicationProjects = new ApplicationProjects(Id);
    }

    public LoanApplicationId Id { get; private set; }

    public UserAccount UserAccount { get; }

    public ApplicationProjects ApplicationProjects { get; private set; }

    public LoanApplicationViewModel LegacyModel { get; set; }

    public static LoanApplicationEntity New(UserAccount userAccount) => new(LoanApplicationId.New(), userAccount);

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
