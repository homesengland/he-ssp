using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

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

        public List<invln_contactwebrole> GetOrganizationIdAndContactId(Guid organizationId, Guid contactId)
        {
            var query = new QueryExpression();
            query.ColumnSet = new ColumnSet(true);
            query.EntityName = invln_contactwebrole.EntityLogicalName;
            query.Criteria.AddCondition(new ConditionExpression(invln_contactwebrole.Fields.invln_Accountid, ConditionOperator.Equal, organizationId));
            query.Criteria.AddCondition(new ConditionExpression(invln_contactwebrole.Fields.invln_Contactid, ConditionOperator.Equal, contactId));

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_contactwebrole>()).ToList();
        }
    }
}
