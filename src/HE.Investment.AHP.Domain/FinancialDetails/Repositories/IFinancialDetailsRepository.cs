using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> GetById(ApplicationId applicationId, CancellationToken cancellationToken);
}
