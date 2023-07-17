using HE.InvestmentLoans.BusinessLogic.Application.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Application.Entities;

public class LoanApplicationEntity
{
    public LoanApplicationEntity(LoanApplicationId id, LoanApplicationViewModel legacyModel)
    {
        Id = id;
        LegacyModel = legacyModel;
    }

    public LoanApplicationId Id { get; }

    public LoanApplicationViewModel LegacyModel { get; }
}
