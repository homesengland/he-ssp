using System.Collections.Generic;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using System.Linq;

namespace HE.CRM.Plugins.Services.LocalAuthority
{
    public class LocalAuthorityService : CrmService, ILocalAuthorityService
    {
        #region Fields
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;
        #endregion

        #region Constructors

        public LocalAuthorityService(CrmServiceArgs args) : base(args)
        {
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
            _heLocalAuthorityRepository = CrmRepositoriesFactory.Get<IHeLocalAuthorityRepository>();
        }
        #endregion

        public List<LocalAuthorityDto> GetAllLocalAuthoritiesAsDto()
        {
            var localAuthorities = _localAuthorityRepository.GetAll();
            var localAuthoritiesDtoList = new List<LocalAuthorityDto>();
            if (localAuthorities.Any())
            {
                foreach(var authority in localAuthorities)
                {
                    localAuthoritiesDtoList.Add(new LocalAuthorityDto()
                    {
                        name = authority.invln_localauthorityname,
                        onsCode = authority.invln_onscode,
                    });
                }
            }
            return localAuthoritiesDtoList;
        }


        public PagedResponseDto<LocalAuthorityDto> GetLocalAuthoritiesForModule(PagingRequestDto pagingRequestDto, string searchPhrase, string module, bool useHeTables)
        {
            if ((module == "loan" || module == "loanFD") && !useHeTables)
            {
                this.TracingService.Trace("module loan or loanFD");
                var result = _localAuthorityRepository.GetLocalAuthoritiesForLoan(pagingRequestDto, searchPhrase);

                return new PagedResponseDto<LocalAuthorityDto>
                {
                    paging = result.paging,
                    totalItemsCount = result.totalItemsCount,
                    items = result.items.Select(i => new LocalAuthorityDto { id = i.invln_localauthorityId.ToString(), name = i.invln_localauthorityname, code = i.invln_onscode }).ToList(),
                };
            }

            if (module == "ahp" && !useHeTables)
            {
                this.TracingService.Trace("module ahp");
                var result = _localAuthorityRepository.GetLocalAuthoritiesForAHP(pagingRequestDto, searchPhrase);

                return new PagedResponseDto<LocalAuthorityDto>
                {
                    paging = result.paging,
                    totalItemsCount = result.totalItemsCount,
                    items = result.items.Select(i => new LocalAuthorityDto { id = i.invln_AHGLocalAuthoritiesId.ToString(), name = i.invln_LocalAuthorityName, code = i.invln_GSSCode }).ToList(),
                };
            }

            if (useHeTables)
            {
                this.TracingService.Trace("module loanFD");
                var result = _heLocalAuthorityRepository.GetLocalAuthoritiesForFdLoan(pagingRequestDto, searchPhrase);

                return new PagedResponseDto<LocalAuthorityDto>
                {
                    paging = result.paging,
                    totalItemsCount = result.totalItemsCount,
                    items = result.items.Select(i => new LocalAuthorityDto { id = i.he_LocalAuthorityId.ToString(), name = i.he_Name, code = i.he_GSSCode }).ToList(),
                };
            }

            return null;
        }
    }
}
