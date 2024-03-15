using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ProgrammeRepository : CrmEntityRepository<invln_programme, DataverseContext>, IProgrammeRepository
    {
        public ProgrammeRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_programme GetProgrammeById(string programmeId)
        {
            var fetchXml =
            @"<fetch>
	            <entity name=""invln_programme"">
		            <filter type=""and"">
			            <condition attribute=""invln_programmeid"" operator=""eq"" value=""" + programmeId + @""" />
		            </filter>
	            </entity>
            </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_programme>()).AsEnumerable().FirstOrDefault();
        }

        public List<invln_programme> GetProgrammes()
        {
            var fetchXml =
            @"<fetch>
	            <entity name=""invln_programme"">
	            </entity>
            </fetch>";

            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_programme>()).AsEnumerable().ToList();
        }

        public List<invln_programme> GetProgrammes(params string[] columns)
        {
            QueryExpression query = new QueryExpression();
            query.ColumnSet = new ColumnSet(columns);
            query.EntityName = invln_programme.EntityLogicalName;
            return this.service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_programme>()).ToList();
        }
    }
}
