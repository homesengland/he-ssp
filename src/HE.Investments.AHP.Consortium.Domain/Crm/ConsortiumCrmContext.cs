using HE.Investments.Common.CRM.Services;

namespace HE.Investments.AHP.Consortium.Domain.Crm;

public class ConsortiumCrmContext : IConsortiumCrmContext
{
    private readonly ICrmService service;

    public ConsortiumCrmContext(ICrmService service)
    {
        this.service = service;
    }

    public void GetConsortium(string consortiumId)
    {
        // Implementation
    }

    public bool IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId)
    {
        return false;
    }

    public void CreateConsortium(string programmeId, string leadOrganisationId)
    {
        // Implementation
    }
}
