using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IFrontDoorProjectSiteRepository : ICrmEntityRepository<invln_FrontDoorProjectSitePOC, DataverseContext>
    {
        List<invln_FrontDoorProjectSitePOC> GetSiteRelatedToFrontDoorProject(EntityReference frontDoorProjectId);
        invln_FrontDoorProjectSitePOC GetSingleFrontDoorProjectSite(string frontDoorSiteId);
        List<invln_FrontDoorProjectSitePOC> GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(string frontDoorProjectId, string organisationId, string externalContactId);
    }
}
