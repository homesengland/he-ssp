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

        public bool IsContactHaveSelectedWebRoleForOrganisation(Guid contactGuid, Guid organisationGuid, invln_Permission permission)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                var portalPermissionLevel = ctx.CreateQuery<invln_portalpermissionlevel>()
                    .SingleOrDefault(x => x.invln_Permission.Value == (int)permission);


                var portalApp = ctx.CreateQuery<invln_portal>()
                    .SingleOrDefault(x => x.invln_Portal.Value == (int)invln_Portal1.Common);


                invln_Webrole webRole = null;
                webRole = ctx.CreateQuery<invln_Webrole>()
                    .Where(x => x.invln_Portalpermissionlevelid.Id == portalPermissionLevel.Id && x.invln_Portalid.Id == portalApp.Id).FirstOrDefault();

                if (webRole == null)
                { return false; }

                return ctx.CreateQuery<invln_contactwebrole>()
                    .Where(x => x.invln_Contactid.Id == contactGuid && x.invln_Accountid.Id == organisationGuid && x.invln_Webroleid.Id == webRole.Id).ToList().Count() > 0;
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
