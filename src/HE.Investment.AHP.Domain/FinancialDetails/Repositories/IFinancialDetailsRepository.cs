using HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.BusinessLogic.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> GetById(FinancialDetailsId financialDetailsId, CancellationToken cancellationToken);
}
