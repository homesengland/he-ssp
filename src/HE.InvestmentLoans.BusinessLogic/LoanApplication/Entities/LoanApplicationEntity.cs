using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

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
        if (Id.IsNew() is false)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }
}
