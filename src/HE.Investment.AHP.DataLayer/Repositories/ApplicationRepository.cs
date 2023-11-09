using HE.Investment.AHP.DataLayer.Mock;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using DomainApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.DataLayer.Repositories;

public class ApplicationRepository : InMemoryRepository<ApplicationEntity>, IApplicationRepository
{
    public ApplicationRepository()
    {
        var id = Guid.NewGuid().ToString();
        _ = Save(new ApplicationEntity(new DomainApplicationId(id), new ApplicationName("App1"), new ApplicationTenure("SocialRent")), CancellationToken.None);
    }

    public async Task<ApplicationEntity> GetById(DomainApplicationId id, CancellationToken cancellationToken)
    {
        return await GetById(id.Value, cancellationToken);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken)
    {
        // TODO: Add name unique validation
        return await Save(application.Id.Value, application, cancellationToken);
    }
}
