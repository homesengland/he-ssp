using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ContactWebroleRepository : CrmEntityRepository<invln_contactwebrole, DataverseContext>, IContactWebroleRepository
    {
        public ContactWebroleRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_contactwebrole> GetAdminContactWebrolesForOrganisation(Guid organisationId, Guid adminWebrole)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_contactwebrole>()
                    .Where(x => x.invln_Accountid.Id == organisationId && x.invln_Webroleid.Id == adminWebrole).ToList();
            }
        }
    }
}
