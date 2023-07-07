using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IWebRoleRepository : ICrmEntityRepository<invln_Webrole, DataverseContext>
    {
        List<invln_contactwebrole> GetContactWebRole(Guid contactId, string portalType);

        invln_Webrole GetRoleByName(string name);
        List<invln_Webrole> GetDefaultPortalRoles(string portalId);
    }
}