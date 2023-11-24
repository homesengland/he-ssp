using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public interface IFinancialDetailsRepository
{
    Task<FinancialDetailsEntity> GetById(ApplicationId id, CancellationToken cancellationToken);

    Task<FinancialDetailsEntity> Save(FinancialDetailsEntity financialDetails, CancellationToken cancellationToken);
}
