using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        public List<invln_FrontDoorProjectPOC> GetFrontDoorProjectForOrganisationAndContact(string organisationCondition, string contactExternalIdFilter, string attributes, string frontDoorProjectFilters, string statecodeCondition)
        {
            logger.Trace("FrontDoorProjectRepository GetFrontDoorProjectForOrganisationAndContact");
            var fetchXML = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_frontdoorprojectpoc'>
                                            <attribute name=""invln_contactid"" />
                                             {attributes}
                                             <filter>
                                              {statecodeCondition}
                                              {organisationCondition}
                                              {frontDoorProjectFilters}
                                             </filter>
                                             <link-entity name=""contact"" from=""contactid"" to=""invln_contactid"">
                                              {contactExternalIdFilter}
                                            </link-entity>
                                          </entity>
                                        </fetch>";
            EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
            return result.Entities.Select(x => x.ToEntity<invln_FrontDoorProjectPOC>()).ToList();
        }

        public List<invln_FrontDoorProjectPOC> GetAccountFrontDoorProjects(Guid accountId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_FrontDoorProjectPOC>()
                    .Where(x => x.invln_AccountId.Id == accountId && x.StatusCode.Value != (int)invln_FrontDoorProjectPOC_StatusCode.Inactive && x.StateCode.Value == (int)invln_FrontDoorProjectPOCState.Active).ToList();
            }
        }

        public bool CheckIfFrontDoorProjectWithGivenNameExists(string frontDoorProjectName)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_FrontDoorProjectPOC>().Where(x => x.invln_Name == frontDoorProjectName && x.StateCode.Value == (int)invln_FrontDoorProjectPOCState.Active).AsEnumerable().Any();
            }
        }
    }
}



