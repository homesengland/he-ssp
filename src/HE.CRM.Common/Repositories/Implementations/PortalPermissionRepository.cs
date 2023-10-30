using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class PortalPermissionRepository : CrmEntityRepository<invln_portalpermissionlevel, DataverseContext>, IPortalPermissionRepository
    {


        public PortalPermissionRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
