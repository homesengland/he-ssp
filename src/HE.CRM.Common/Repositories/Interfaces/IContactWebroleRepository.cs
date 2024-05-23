using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IContactWebroleRepository : ICrmEntityRepository<invln_contactwebrole, DataverseContext>
    {
        List<invln_contactwebrole> GetAdminContactWebrolesForOrganisation(Guid organisationId, Guid adminWebrole);
        bool IsContactHaveSelectedWebRoleForOrganisation(Guid contactGuid, Guid organisationGuid, invln_Permission permission);
        List<invln_contactwebrole> GetOrganizationIdAndContactId(Guid organizationId, Guid contactId);
    }
}