using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IConsortiumPartnerStatusProvider
{
    public Task<ConsortiumPartnerStatus> GetConsortiumPartnerStatus(
        ConsortiumId consortiumId,
        OrganisationId organisationId,
        CancellationToken cancellationToken);
}
