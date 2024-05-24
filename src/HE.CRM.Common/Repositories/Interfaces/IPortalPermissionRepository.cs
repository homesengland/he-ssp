using DataverseModel;
using HE.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Common.Repositories.interfaces
{
    public interface IPortalPermissionRepository : ICrmEntityRepository<invln_portalpermissionlevel, DataverseContext>
    {
        List<invln_portalpermissionlevel> GetByAccountAndContact(Guid accountId, Guid contactId);
    }
}
