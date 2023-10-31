using HE.Investment.AHP.Domain.Application.Entities;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application;

public interface IApplicationRepository
{
    Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken);

    Task<IList<ApplicationEntity>> GetAll(CancellationToken cancellationToken);

    Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken);
}
