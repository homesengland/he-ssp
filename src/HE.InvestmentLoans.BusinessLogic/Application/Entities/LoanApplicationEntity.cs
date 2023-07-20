using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Application.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, UserAccount userAccount)
    {
        Id = id;
        UserAccount = userAccount;
    }

    public LoanApplicationId Id { get; private set; }

    public UserAccount UserAccount { get; }

    public LoanApplicationViewModel LegacyModel { get; private set; }

    public static LoanApplicationEntity New(UserAccount userAccount) => new(LoanApplicationId.New(), userAccount);

    public void SetId(LoanApplicationId id)
    {
        if (Id.IsNew() is false)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = id;
    }
}
