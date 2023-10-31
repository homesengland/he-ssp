using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Mock;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : InMemoryRepository<ApplicationEntity>, IApplicationRepository
{
    public async Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        return await GetById(id.Value, cancellationToken);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken)
    {
        return await Save(application.Id.Value, application, cancellationToken);
    }
}
