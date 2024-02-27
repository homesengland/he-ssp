using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhgLocalAuthorityRepository : CrmEntityRepository<invln_AHGLocalAuthorities, DataverseContext>, IAhgLocalAuthorityRepository
    {
        public AhgLocalAuthorityRepository(CrmRepositoryArgs args) : base((CrmRepositoryArgs)args)
        {
        }

        public List<invln_AHGLocalAuthorities> GetAll()
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_AHGLocalAuthorities>().ToList();
            }
        }

        public invln_AHGLocalAuthorities GetLocalAuthorityWithGivenCode(string dode)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_AHGLocalAuthorities>().FirstOrDefault(x => x.invln_GSSCode == dode);
            }
        }
    }
}
