namespace HE.Investments.AHP.Consortium.Domain.Crm;

public interface IConsortiumCrmContext
{
    void GetConsortium(string consortiumId);
    bool IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId);
    void CreateConsortium(string programmeId, string leadOrganisationId);
}
