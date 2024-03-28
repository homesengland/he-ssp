using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IAhgLocalAuthorityRepository : ICrmEntityRepository<invln_AHGLocalAuthorities, DataverseContext>
    {
        PagedResponseDto<invln_AHGLocalAuthorities> Get(PagingRequestDto pagingRequestDto, string searchPhrase, string fieldsToRetrieve);
        invln_AHGLocalAuthorities GetLocalAuthorityWithGivenCode(string code);
        invln_AHGLocalAuthorities GetAhpLocalAuthoritiesReletedToSite(Guid siteId);
    }
}
