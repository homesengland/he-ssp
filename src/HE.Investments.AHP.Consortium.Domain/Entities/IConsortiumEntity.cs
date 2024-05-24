using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public interface IConsortiumEntity
{
    Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium, CancellationToken cancellationToken);

    Task RemoveMember(
        OrganisationId organisationId,
        bool? isConfirmed,
        IConsortiumPartnerStatusProvider consortiumPartnerStatusProvider,
        CancellationToken cancellationToken);
}
