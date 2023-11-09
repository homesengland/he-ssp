using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Mock;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : InMemoryRepository<ApplicationEntity>, IApplicationRepository
{
    public ApplicationRepository()
    {
        var id = Guid.NewGuid().ToString();
        _ = Save(new ApplicationEntity(new ApplicationId(id), new ApplicationName("App1"), new ApplicationTenure("SocialRent")), CancellationToken.None);
    }

    public async Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        return await GetById(id.Value, cancellationToken);
    }

    public async Task<ApplicationBasicInfo> GetApplicationBasicInfo(ApplicationId id, CancellationToken cancellationToken)
    {
        var application = await GetById(id, cancellationToken);
        return new ApplicationBasicInfo(application.Id, application.Tenure?.Value ?? Tenure.Undefined, ApplicationStatus.Draft);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken)
    {
        // TODO: Add name unique validation
        return await Save(application.Id.Value, application, cancellationToken);
    }
}
