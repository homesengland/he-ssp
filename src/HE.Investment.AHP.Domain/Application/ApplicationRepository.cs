using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application;

public class ApplicationRepository : IApplicationRepository
{
    private static readonly IDictionary<string, ApplicationEntity> Applications = new ConcurrentDictionary<string, ApplicationEntity>();

    public Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        var item = Get(id);
        if (item != null)
        {
            return Task.FromResult(item);
        }

        throw new NotFoundException(nameof(ApplicationEntity), id);
    }

    public async Task<IList<ApplicationEntity>> GetAll(CancellationToken cancellationToken)
    {
        return await Task.FromResult(Applications.Values.ToList());
    }

    public Task<ApplicationEntity> Save(
        ApplicationEntity application,
        CancellationToken cancellationToken)
    {
        Save(application);
        return Task.FromResult(application);
    }

    private ApplicationEntity? Get(ApplicationId applicationId)
    {
        if (Applications.TryGetValue(applicationId.ToString(), out var entity))
        {
            return entity;
        }

        return null;
    }

    private void Save(ApplicationEntity entity)
    {
        if (Applications.ContainsKey(entity.Id!.Value))
        {
            Applications.Remove(entity.Id!.Value);
        }

        Applications[entity.Id!.Value] = entity;
    }
}
