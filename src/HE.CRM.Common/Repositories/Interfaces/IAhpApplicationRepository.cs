using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IAhpApplicationRepository : ICrmEntityRepository<invln_scheme, DataverseContext>
    {
        List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactId, string attributes);
    }
}
