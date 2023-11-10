using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.InvestmentLoans.Common.Exceptions;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private static readonly IDictionary<Guid, FinancialDetailsEntity> FinancialDetails = new ConcurrentDictionary<Guid, FinancialDetailsEntity>();

    public Task<FinancialDetailsEntity> GetById(ApplicationId applicationId, CancellationToken cancellationToken)
    {
        var financialDetails = Get(applicationId.Value);
        if (financialDetails != null)
        {
            return Task.FromResult(financialDetails);
        }

        throw new NotFoundException(nameof(FinancialDetailsEntity), applicationId);
    }

    public Task SaveAsync(FinancialDetailsEntity financialDetailsEntity, CancellationToken cancellationToken)
    {
        return Task.Run(() => Save(financialDetailsEntity), cancellationToken);
    }

    private FinancialDetailsEntity? Get(Guid financialDetailsId)
    {
        if (FinancialDetails.TryGetValue(financialDetailsId, out var financialDetails))
        {
            return financialDetails;
        }

        return null;
    }

    private void Save(FinancialDetailsEntity financialDetailsEntity)
    {
        if (FinancialDetails.TryGetValue(financialDetailsEntity.ApplicationId.Value, out var financialDetails))
        {
            var existingFinancialDetails = financialDetails;
            if (existingFinancialDetails != null)
            {
                FinancialDetails.Remove(financialDetails.ApplicationId.Value);
            }
        }

        FinancialDetails.Add(financialDetailsEntity.ApplicationId.Value, financialDetailsEntity);
    }
}
