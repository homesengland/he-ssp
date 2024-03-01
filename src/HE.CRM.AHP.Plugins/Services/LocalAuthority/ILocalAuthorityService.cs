using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.LocalAuthority
{
    public interface ILocalAuthorityService : ICrmService
    {
        PagedResponseDto<AhgLocalAuthorityDto> Get(PagingRequestDto pagingRequestDto, string searchPhrase, string fieldsToRetrieve);
    }
}
