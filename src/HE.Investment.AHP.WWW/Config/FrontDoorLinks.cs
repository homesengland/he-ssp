using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Config;

public class FrontDoorLinks : IFrontDoorLinks
{
    private readonly IConfiguration _configuration;

    private readonly OrganisationId _selectedOrganisationId;

    public FrontDoorLinks(IConfiguration configuration, IAccountUserContext accountUserContext)
    {
        _configuration = configuration;
        _selectedOrganisationId = accountUserContext.GetSelectedAccount().Result.SelectedOrganisationId();
    }

    public string StartNewProject => _configuration
        .GetValue<string>("AppConfiguration:ProgrammeUrl:StartFrontDoorProject") ?? string.Empty
        .Replace("{organisationId}", _selectedOrganisationId.ToString());
}
