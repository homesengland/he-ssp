using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ILocalAuthorityRepository : ICrmEntityRepository<invln_localauthority, DataverseContext>
    {
        List<invln_localauthority> GetAll();
        invln_localauthority GetLocalAuthorityWithGivenOnsCode(string onsCode);

        PagedResponseDto<invln_localauthority> GetLocalAuthoritiesForLoan(PagingRequestDto pagingRequestDto, string searchPhrase);
        PagedResponseDto<invln_AHGLocalAuthorities> GetLocalAuthoritiesForAHP(PagingRequestDto pagingRequestDto, string searchPhrase);
    }
}
