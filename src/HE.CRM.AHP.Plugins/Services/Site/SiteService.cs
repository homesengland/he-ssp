using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml.Linq;
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

        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;



        public SiteService(CrmServiceArgs args) : base(args)
        {
            _repository = CrmRepositoriesFactory.Get<ISiteRepository>();
            _localAuthorityRepository = CrmRepositoriesFactory.Get<IAhgLocalAuthorityRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();

            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }

        public PagedResponseDto<SiteDto> GetMultiple(PagingRequestDto paging, string fieldsToRetrieve, string externalContactId, string accountId)
        {
            TracingService.Trace($"SiteService GetMultiple");
            var externalContactIdFilter = GetFetchXmlConditionForGivenField(externalContactId, nameof(Contact.invln_externalid).ToLower());
            externalContactIdFilter = GenerateFilterMarksForCondition(externalContactIdFilter);

            var accountIdFilter = GetFetchXmlConditionForGivenField(accountId, nameof(invln_Sites.invln_AccountId).ToLower());
            accountIdFilter = GenerateFilterMarksForCondition(accountIdFilter);

            var result = _repository.GetMultiple(paging, fieldsToRetrieve, externalContactIdFilter, accountIdFilter);

            List<SiteDto> siteDtoList = new List<SiteDto>();
            foreach (var site in result.items)
            {
                he_LocalAuthority localAuth = null;
                if (site.invln_HeLocalAuthorityId != null)
                {
                    localAuth = _heLocalAuthorityRepository.GetById(site.invln_HeLocalAuthorityId.Id, new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });

                }
                siteDtoList.Add(SiteMapper.ToDto(site, localAuth));
            }
            return new PagedResponseDto<SiteDto>
            {
                paging = paging,
                items = siteDtoList,
                totalItemsCount = result.totalItemsCount,
            };
        }

        public SiteDto GetSingle(string id, string fieldsToRetrieve, string externalContactId, string accountId)
        {
            TracingService.Trace($"SiteService GetSingle");

            var siteIdFilter = GetFetchXmlConditionForGivenField(id, nameof(invln_Sites.invln_SitesId).ToLower());
            siteIdFilter = GenerateFilterMarksForCondition(siteIdFilter);

            var externalContactIdFilter = GetFetchXmlConditionForGivenField(externalContactId, nameof(Contact.invln_externalid).ToLower());
            externalContactIdFilter = GenerateFilterMarksForCondition(externalContactIdFilter);

            var accountIdFilter = GetFetchXmlConditionForGivenField(accountId, nameof(invln_Sites.invln_AccountId).ToLower());
            accountIdFilter = GenerateFilterMarksForCondition(accountIdFilter);

            var site = _repository.GetSingle(siteIdFilter, fieldsToRetrieve, externalContactIdFilter, accountIdFilter);

            he_LocalAuthority localAuth = null;
            if (site.invln_HeLocalAuthorityId != null)
            {
                localAuth = _heLocalAuthorityRepository.GetById(site.invln_HeLocalAuthorityId.Id, new string[] { nameof(he_LocalAuthority.he_LocalAuthorityId).ToLower(), nameof(he_LocalAuthority.he_Name).ToLower(), nameof(he_LocalAuthority.he_GSSCode).ToLower() });
            }

            return SiteMapper.ToDto(site, localAuth);
        }

        public bool Exist(string name)
        {
            return _repository.Exist(name);
        }

        public bool StrategicSiteNameExists(string strategicSiteName, Guid organisationGuid)
        {
            TracingService.Trace($"SiteService StrategicSiteNameExists");
            return _repository.StrategicSiteNameExists(strategicSiteName, organisationGuid);
        }

        public string Save(string siteId, SiteDto site, string fieldsToSet, string externalContactId, string accountId)
        {
            TracingService.Trace($"SiteService Save");
            he_LocalAuthority heLocalAuthority = null;
            if (!string.IsNullOrWhiteSpace(site.localAuthority?.id))
            {
                heLocalAuthority = _heLocalAuthorityRepository.GetLocalAuthorityWithGivenCode(site.localAuthority.id);
            }

            Contact createdByContact = null;
            if (!string.IsNullOrEmpty(externalContactId))
            {
                createdByContact = _contactRepository.GetContactViaExternalId(externalContactId);
            }

            var entity = SiteMapper.ToEntity(site, fieldsToSet, heLocalAuthority, createdByContact, accountId, siteId);

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
