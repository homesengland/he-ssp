using System.Linq;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Services.LocalAuthority
{
    public class LocalAuthorityService :  CrmService, ILocalAuthorityService
    {
        private readonly IAhgLocalAuthorityRepository _repository;

        public LocalAuthorityService(CrmServiceArgs args) : base(args)
        {
            _repository = CrmRepositoriesFactory.Get<IAhgLocalAuthorityRepository>();
        }

        public PagedResponseDto<AhgLocalAuthorityDto> Get(PagingRequestDto pagingRequestDto, string searchPhrase, string fieldsToRetrieve)
        {
            var result= _repository.Get(pagingRequestDto, searchPhrase, fieldsToRetrieve);

            return new PagedResponseDto<AhgLocalAuthorityDto>
            {
                paging = result.paging,
                totalItemsCount = result.totalItemsCount,
                items = result.items.Select(i => new AhgLocalAuthorityDto{ id = i.invln_GSSCode, name = i.invln_LocalAuthorityName}).ToList(),
            };
        }
    }
}
