using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class PortalPermissionRepository : CrmEntityRepository<invln_portalpermissionlevel, DataverseContext>, IPortalPermissionRepository
    {


        public PortalPermissionRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
