using System.Collections.Concurrent;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Repositories;

public class FinancialDetailsRepository : IFinancialDetailsRepository
{
    private static readonly IDictionary<Guid, FinancialDetailsEntity> FinancialDetails = new ConcurrentDictionary<Guid, FinancialDetailsEntity>();

    public Task<FinancialDetailsEntity> GetById(FinancialDetailsId financialDetailsId, CancellationToken cancellationToken)
    {
        var financialDetails = Get(financialDetailsId.Value);
        if (financialDetails != null)
        {
            return Task.FromResult(financialDetails);
        }

        throw new NotFoundException(nameof(FinancialDetailsEntity), financialDetailsId);
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
        if (FinancialDetails.TryGetValue(financialDetailsEntity.FinancialDetailsId.Value, out var financialDetails))
        {
            var existingFinancialDetails = financialDetails;
            if (existingFinancialDetails != null)
            {
                FinancialDetails.Remove(financialDetails.FinancialDetailsId.Value);
            }
        }

        FinancialDetails.Add(financialDetailsEntity.FinancialDetailsId.Value, financialDetailsEntity);
    }
}
