using System.Collections.Generic;
using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.interfaces;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class PortalPermissionRepository : CrmEntityRepository<invln_portalpermissionlevel, DataverseContext>, IPortalPermissionRepository
    {
        public PortalPermissionRepository(CrmRepositoryArgs args) : base(args)
        {
        }
        public List<invln_portalpermissionlevel> GetByAccountAndContact(Guid accountId, Guid contactId)
        {
            var query = new QueryExpression(invln_portalpermissionlevel.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_portalpermissionlevel.Fields.invln_name,
                invln_portalpermissionlevel.Fields.invln_Permission);
            var query_invln_webrole = query.AddLink(
                invln_Webrole.EntityLogicalName,
                invln_portalpermissionlevel.Fields.invln_portalpermissionlevelId,
                invln_Webrole.Fields.invln_Portalpermissionlevelid);

            var query_invln_webrole_invln_contactwebrole = query_invln_webrole.AddLink(
                invln_contactwebrole.EntityLogicalName,
                invln_Webrole.Fields.invln_WebroleId,
                invln_contactwebrole.Fields.invln_Webroleid);

            query_invln_webrole_invln_contactwebrole.LinkCriteria.AddCondition(invln_contactwebrole.Fields.invln_Accountid, ConditionOperator.Equal, accountId);
            query_invln_webrole_invln_contactwebrole.LinkCriteria.AddCondition(invln_contactwebrole.Fields.invln_Contactid, ConditionOperator.Equal, contactId);
            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_portalpermissionlevel>()).ToList();
        }
    }
}
