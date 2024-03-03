using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;


namespace HE.CRM.Common.Repositories.Implementations
{
    public class FrontDoorProjectSiteRepository : CrmEntityRepository<invln_FrontDoorProjectSitePOC, DataverseContext>, IFrontDoorProjectSiteRepository
    {
        public FrontDoorProjectSiteRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_FrontDoorProjectSitePOC> GetSiteRelatedToFrontDoorProject(EntityReference frontDoorProjectId)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                return ctx.CreateQuery<invln_FrontDoorProjectSitePOC>()
                    .Where(x => x.invln_FrontDoorProjectId.Id == frontDoorProjectId.Id && x.StateCode.Value == (int)invln_FrontDoorProjectSitePOCState.Active).ToList();
            }
        }

        // Maybe it is unnecessary

        //public invln_FrontDoorProjectSitePOC GetSingleSiteRelatedToFrontDoorProject(EntityReference frontDoorProjectId, string frontDoorSiteId)
        //{
        //    if (Guid.TryParse(frontDoorSiteId, out Guid siteId))
        //    {
        //        using (var ctx = new OrganizationServiceContext(service))
        //        {
        //            return ctx.CreateQuery<invln_FrontDoorProjectSitePOC>().FirstOrDefault(x => x.Id == siteId && x.invln_FrontDoorProjectId.Id == frontDoorProjectId.Id && x.StateCode.Value == (int)invln_FrontDoorProjectSitePOCState.Active);
        //        }
        //    }
        //    else
        //    {
        //        return new invln_FrontDoorProjectSitePOC();
        //    }
        //}

        public List<invln_FrontDoorProjectSitePOC> GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(string frontDoorProjectId, string organisationId, string externalContactId)
        {
            var contactId = string.Empty;
            using (var ctx = new OrganizationServiceContext(service))
            {
                contactId = ctx.CreateQuery<Contact>().FirstOrDefault(x => x.invln_externalid == externalContactId).Id.ToString();
            }
            logger.Trace($"contactid: {contactId}");
            logger.Trace($"organisationId: {organisationId}");



            if (Guid.TryParse(frontDoorProjectId, out Guid frontDoorProjecGuid) && Guid.TryParse(organisationId, out Guid organisationGuid))
            {
                var fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='invln_frontdoorprojectsitepoc'>
                                            <filter>
                                              <condition attribute=""statuscode"" operator=""eq"" value=""1"" />
                                              <condition entityname=""FrontDoorProject"" attribute=""invln_accountid"" operator=""eq"" value=""" + organisationId + @""" />
                                              <condition entityname=""FrontDoorProject"" attribute=""invln_contactid"" operator=""eq"" value=""" + contactId + @""" />
                                              <condition attribute=""invln_frontdoorprojectid"" operator=""eq"" value=""" + frontDoorProjectId + @""" />
                                            </filter>
		                                    <link-entity name=""invln_frontdoorprojectpoc"" from=""invln_frontdoorprojectpocid"" to=""invln_frontdoorprojectid"" alias=""FrontDoorProject"">
		                                    </link-entity>
                                          </entity>
                                        </fetch>";

                EntityCollection result = service.RetrieveMultiple(new FetchExpression(fetchXML));
                return result.Entities.Select(x => x.ToEntity<invln_FrontDoorProjectSitePOC>()).ToList();
            }
            else
            {
                return new List<invln_FrontDoorProjectSitePOC>();
            }
        }


        public invln_FrontDoorProjectSitePOC GetSingleFrontDoorProjectSite(string frontDoorSiteId)
        {
            if (Guid.TryParse(frontDoorSiteId, out Guid siteId))
            {
                using (var ctx = new OrganizationServiceContext(service))
                {
                    return ctx.CreateQuery<invln_FrontDoorProjectSitePOC>().FirstOrDefault(x => x.Id == siteId);
                }
            }
            else
            {
                return new invln_FrontDoorProjectSitePOC();
            }
        }

    }
}
