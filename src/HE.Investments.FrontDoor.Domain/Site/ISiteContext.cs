using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Site;

public interface ISiteContext
{
    Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken);

    Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken);
}
