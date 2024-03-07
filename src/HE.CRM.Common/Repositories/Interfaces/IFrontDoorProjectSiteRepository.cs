using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IFrontDoorProjectSiteRepository : ICrmEntityRepository<invln_FrontDoorProjectSitePOC, DataverseContext>
    {
        PagedResponseDto<invln_FrontDoorProjectSitePOC> GetFrontDoorProjectSites(PagingRequestDto paging, string frontDoorProjectIdCondition, string attributes);
        invln_FrontDoorProjectSitePOC GetFrontDoorProjectSite(string frontDoorProjectIdCondition, string frontDoorProjecSitetIdCondition, string attributes);
    }
}
