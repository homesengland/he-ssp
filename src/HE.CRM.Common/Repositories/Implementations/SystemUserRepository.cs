using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class SystemUserRepository : CrmEntityRepository<SystemUser, DataverseContext>, ISystemUserRepository
    {
        public SystemUserRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
