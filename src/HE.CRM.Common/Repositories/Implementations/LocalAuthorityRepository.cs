using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class LocalAuthorityRepository : CrmEntityRepository<invln_localauthority, DataverseContext>, ILocalAuthorityRepository
    {
        #region Constructors

        public LocalAuthorityRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_localauthority> GetAll()
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_localauthority>().ToList();
            }
        }

        public invln_localauthority GetLocalAuthorityWithGivenOnsCode(string onsCode)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_localauthority>()
                    .Where(x => x.invln_onscode == onsCode).FirstOrDefault();
            }
        }
        #endregion
    }
}
