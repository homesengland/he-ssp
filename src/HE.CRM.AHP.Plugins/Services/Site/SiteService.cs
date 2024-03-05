using System.Linq;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Services.Site
{
    public class SiteService : CrmService, ISiteService
    {
        private readonly ISiteRepository _repository;
        private readonly IAhgLocalAuthorityRepository _localAuthorityRepository;

        public SiteService(CrmServiceArgs args) : base(args)
        {
            _repository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<IAhgLocalAuthorityRepository>();
        }
        public PagedResponseDto<SiteDto> Get(PagingRequestDto paging, string fieldsToRetrieve)
        {
            var result = _repository.Get(paging, fieldsToRetrieve);

            return new PagedResponseDto<SiteDto>
            {
                paging = paging,
                items = result.items.Select(SiteMapper.ToDto).ToList(),
                totalItemsCount = result.totalItemsCount,
            };
        }

        public SiteDto GetById(string id, string fieldsToRetrieve)
        {
            var site = _repository.GetById(id, fieldsToRetrieve);
            return SiteMapper.ToDto(site);
        }

        public bool Exist(string name)
        {
            return _repository.Exist(name);
        }

        public string Save(string siteId, SiteDto site, string fieldsToSet)
        {
            invln_AHGLocalAuthorities localAuth = null;
            if (!string.IsNullOrWhiteSpace(site.localAuthority.id))
            {
                localAuth = _localAuthorityRepository.GetLocalAuthorityWithGivenCode(site.localAuthority.id);
            }

            var entity = SiteMapper.ToEntity(site, fieldsToSet, localAuth);

            if (string.IsNullOrEmpty(siteId))
            {
                var id = _repository.Create(entity);
                return id.ToString();
            }

            _repository.Update(entity);

            return siteId;
        }
    }
}
