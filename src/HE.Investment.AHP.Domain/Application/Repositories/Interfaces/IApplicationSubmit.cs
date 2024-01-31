using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.Repositories.Interfaces;

public interface IApplicationSubmit
{
    Task Submit(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken);
}
