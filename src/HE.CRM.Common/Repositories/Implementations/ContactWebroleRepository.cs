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

        public List<invln_contactwebrole> GetListOfUsersWithoutLimitedRole(string organizationId)
        {
            var query_invln_accountid = organizationId;
            var webroleid_invln_name = "Limited user";

            var query = new QueryExpression(invln_contactwebrole.EntityLogicalName);
            query.ColumnSet.AddColumns(invln_contactwebrole.Fields.invln_Accountid, invln_contactwebrole.Fields.invln_Contactid);
            query.Criteria.AddCondition(invln_contactwebrole.Fields.invln_Accountid, ConditionOperator.Equal, query_invln_accountid);
            var webroleid = query.AddLink(
                invln_Webrole.EntityLogicalName,
                invln_contactwebrole.Fields.invln_Webroleid,
                invln_Webrole.Fields.invln_WebroleId);
            webroleid.EntityAlias = "webroleid";
            webroleid.Columns.AddColumn(invln_Webrole.Fields.invln_Name);
            webroleid.LinkCriteria.AddCondition(invln_Webrole.Fields.invln_Name, ConditionOperator.NotEqual, webroleid_invln_name);
            var contactid = query.AddLink(Contact.EntityLogicalName, invln_contactwebrole.Fields.invln_Contactid, Contact.Fields.ContactId);
            contactid.EntityAlias = "contactid";
            contactid.Columns.AddColumn(Contact.Fields.invln_externalid);

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_contactwebrole>()).ToList();
        }
    }
}
