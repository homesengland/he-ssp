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
        private readonly IContactRepository _contactRepository;



        public SiteService(CrmServiceArgs args) : base(args)
        {
            _repository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<IAhgLocalAuthorityRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
        }




        public PagedResponseDto<SiteDto> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactId, string accountId)
        {
            var externalContactIdFilter = GetFetchXmlConditionForGivenField(externalContactId, nameof(Contact.invln_externalid).ToLower());
            externalContactIdFilter = GenerateFilterMarksForCondition(externalContactIdFilter);
            var accountIdFilter = GetFetchXmlConditionForGivenField(accountId, nameof(invln_Sites.invln_AccountId).ToLower());
            accountIdFilter = GenerateFilterMarksForCondition(accountIdFilter);

            var result = _repository.GetMultiple(paging, fieldsToRetrieve, externalContactIdFilter, accountIdFilter);

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

        public string Save(string siteId, SiteDto site, string fieldsToSet, string externalContactId, string accountId)
        {
            TracingService.Trace($"SiteService Save");
            invln_AHGLocalAuthorities localAuth = null;
            if (!string.IsNullOrWhiteSpace(site.localAuthority.id))
            {
                localAuth = _localAuthorityRepository.GetLocalAuthorityWithGivenCode(site.localAuthority.id);
            }

            Contact createdByContact = null;
            if (!string.IsNullOrEmpty(externalContactId))
            {
                createdByContact = _contactRepository.GetContactViaExternalId(externalContactId);
            }
            var entity = SiteMapper.ToEntity(site, fieldsToSet, localAuth, createdByContact, accountId);

            if (string.IsNullOrEmpty(siteId))
            {
                var id = _repository.Create(entity);
                return id.ToString();
            }

            _repository.Update(entity);

            return siteId;
        }

        private string GetFetchXmlConditionForGivenField(string fieldValue, string fieldName)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                return $"<condition attribute=\"{fieldName}\" operator=\"eq\" value=\"{fieldValue}\" />";
            }
            return string.Empty;
        }

        private string GenerateFilterMarksForCondition(string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                return $"<filter>{condition}</filter>";
            }
            return string.Empty;
        }
    }
}
