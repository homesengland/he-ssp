using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.Repositories.Interfaces;

public interface IChangeApplicationStatus
{
    Task ChangeApplicationStatus(ApplicationEntity application, OrganisationId organisationId, string? changeReason, CancellationToken cancellationToken);
}
