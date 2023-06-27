using System;
using HE.Base.Repositories;
using DataverseModel;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IWebRoleRepository : ICrmEntityRepository<invln_Webrole, DataverseContext>
    {
        invln_Webrole GetContactRole(Guid contactId, Guid portalId);

        invln_Webrole GetRoleByName(string name);
    }
}