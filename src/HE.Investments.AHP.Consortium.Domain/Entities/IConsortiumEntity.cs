extern alias Org;

using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using InvestmentsOrganisation = Org::HE.Investments.Organisation.ValueObjects.InvestmentsOrganisation;

namespace HE.Investments.AHP.Consortium.Domain.Entities;

public interface IConsortiumEntity
{
    Task AddMember(InvestmentsOrganisation organisation, IIsPartOfConsortium isPartOfConsortium, CancellationToken cancellationToken);

    void RemoveMember(OrganisationId organisationId, bool? isConfirmed);
}
