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
    public class FrontDoorProjectRepository : CrmEntityRepository<invln_FrontDoorProjectPOC, DataverseContext>, IFrontDoorProjectRepository
    {
        public FrontDoorProjectRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_FrontDoorProjectPOC> GetFrontDoorProjectForOrganisationAndContact(Guid organisationId, string externalContactId, string frontDoorProjectId = null, string fieldsToRetrieve = null)
        {
            if (frontDoorProjectId == null)
            {
                using (DataverseContext ctx = new DataverseContext(service))
                {
                    return (from fdp in ctx.invln_FrontDoorProjectPOCSet
                            join cnt in ctx.ContactSet on fdp.invln_ContactId.Id equals cnt.ContactId
                            where fdp.invln_AccountId.Id == organisationId && cnt.invln_externalid == externalContactId &&
                            fdp.StatusCode.Value != (int)invln_FrontDoorProjectPOC_StatusCode.Inactive
                            select fdp).ToList();
                }
            }
            else
            {
                if (Guid.TryParse(frontDoorProjectId, out Guid frontDoorProjecGuid))
                {
                    var fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_frontdoorprojectpoc'>"
                                    + fieldsToRetrieve +
                                    @"<filter>
                                              <condition attribute=""statuscode"" operator=""ne"" value=""2"" />
                                              <condition attribute=""invln_accountid"" operator=""eq"" value=""" + organisationId + @""" />
                                              <condition attribute=""invln_frontdoorprojectpocid"" operator=""eq"" value=""" + frontDoorProjectId + @""" />
                                            </filter>
                                            <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                              <filter>
                                                <condition attribute=""invln_externalid"" operator=""eq"" value=""" + externalContactId + @""" />
                                              </filter>
                                            </link-entity>
                                          </entity>
                                        </fetch>";
                    EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
                    return result.Entities.Select(x => x.ToEntity<invln_FrontDoorProjectPOC>()).ToList();
                }
                else
                {
                    return new List<invln_FrontDoorProjectPOC>();
                }
            }
        }

        public List<invln_FrontDoorProjectPOC> GetAccountFrontDoorProjects(Guid accountId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_FrontDoorProjectPOC>()
                    .Where(x => x.invln_AccountId.Id == accountId && x.StatusCode.Value != (int)invln_FrontDoorProjectPOC_StatusCode.Inactive && x.StateCode.Value != (int)invln_FrontDoorProjectPOCState.Inactive).ToList();
            }
        }
    }
}
