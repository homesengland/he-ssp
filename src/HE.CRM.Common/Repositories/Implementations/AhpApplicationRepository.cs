using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class AhpApplicationRepository : CrmEntityRepository<invln_scheme, DataverseContext>, IAhpApplicationRepository
    {
        public AhpApplicationRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_scheme> GetApplicationsForOrganisationAndContact(string organisationId, string contactId, string attributes)
        {
            var fetchXml = @"<fetch>
                              <entity name=""invln_scheme"">"
                                + attributes +
                              @"</entity>
                            </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return result.Entities.Select(x => x.ToEntity<invln_scheme>()).ToList();
        }
    }
}
