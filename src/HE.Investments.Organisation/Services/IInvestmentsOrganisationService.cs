using HE.Investments.Organisation.Entities;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.Organisation.Services;

public interface IInvestmentsOrganisationService
{
    Task<InvestmentsOrganisation> GetOrganisation(OrganisationIdentifier organisationIdentifier, CancellationToken cancellationToken);

    InvestmentsOrganisation CreateOrganisation(IManualOrganisation organisation);
}
