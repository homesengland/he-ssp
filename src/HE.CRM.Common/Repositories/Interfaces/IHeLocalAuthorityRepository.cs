using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;
using Microsoft.Xrm.Sdk;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IHeLocalAuthorityRepository : ICrmEntityRepository<he_LocalAuthority, DataverseContext>
    {
        he_LocalAuthority GetLocalAuthorityWithGivenCode(string code);

        PagedResponseDto<he_LocalAuthority> GetLocalAuthoritiesForFdLoan(PagingRequestDto pagingRequestDto, string searchPhrase);
        he_LocalAuthority GetHeLocalAuthorityrelatedToLoanApplication(Guid entityReference);
    }
}
