using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Services.LocalAuthority
{
    public interface ILocalAuthorityService : ICrmService
    {
        List<LocalAuthorityDto> GetAllLocalAuthoritiesAsDto();
    }
}
