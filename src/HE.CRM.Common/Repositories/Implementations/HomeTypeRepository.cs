using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class HomeTypeRepository : CrmEntityRepository<invln_HomeType, DataverseContext>, IHomeTypeRepository
    {
        public HomeTypeRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_HomeType> GetHomeTypesRelatedToApplication(Guid applicationId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_HomeType>()
                     .Where(x => x.invln_application.Id == applicationId).ToList();
            }
        }
    }
}
