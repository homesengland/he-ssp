using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhpProjectRepository : CrmEntityRepository<invln_ahpproject, DataverseContext>, IAhpProjectRepository
    {
        public AhpProjectRepository(CrmRepositoryArgs args) : base(args)
        {
        }
    }
}
