using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public class CurrentOrganisationRepository : ICurrentOrganisationRepository
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IAccountUserContext _accountUserContext;

    public CurrentOrganisationRepository(IOrganizationRepository organizationRepository, IAccountUserContext accountUserContext)
    {
        _organizationRepository = organizationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OrganizationBasicInformation> GetBasicInformation(CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return await _organizationRepository.GetBasicInformation(account, cancellationToken);
    }
}
