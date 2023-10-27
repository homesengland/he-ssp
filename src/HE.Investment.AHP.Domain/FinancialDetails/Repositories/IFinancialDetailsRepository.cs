using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> GetById(FinancialDetailsId financialDetailsId, CancellationToken cancellationToken);
}
