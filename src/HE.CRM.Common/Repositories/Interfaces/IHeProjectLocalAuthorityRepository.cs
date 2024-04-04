using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IHeProjectLocalAuthorityRepository : ICrmEntityRepository<he_ProjectLocalAuthority, DataverseContext>
    {
        PagedResponseDto<he_ProjectLocalAuthority> HeGetFrontDoorProjectSites(PagingRequestDto paging, string frontDoorProjectIdCondition);
        he_ProjectLocalAuthority HeGetFrontDoorProjectSite(string frontDoorProjectIdCondition, string frontDoorProjecSitetIdCondition);
    }
}
