using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> GetById(ApplicationId applicationId, CancellationToken cancellationToken);
}
