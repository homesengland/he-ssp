using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    internal class MilestoneFrameworkItemRepository : CrmEntityRepository<invln_milestoneframeworkitem, DataverseContext>, IMilestoneFrameworkItemRepository
    {
        public MilestoneFrameworkItemRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_milestoneframeworkitem> GetMilestoneFrameworkItemByProgrammeId(string programmeId)
        {
            var fetchXml =
            @"<fetch>
	            <entity name=""invln_milestoneframeworkitem"">
		            <filter type=""and"">
			            <condition attribute=""invln_programmeid"" operator=""eq"" value=""" + programmeId + @""" />
		            </filter>
	            </entity>
            </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            var milestoneFrameworkItem =  result.Entities.Select(x => x.ToEntity<invln_milestoneframeworkitem>()).AsEnumerable().ToList();
            return milestoneFrameworkItem;
        }
    }
}
