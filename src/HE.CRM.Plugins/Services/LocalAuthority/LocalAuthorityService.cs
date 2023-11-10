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
        #endregion

        #region Constructors

        public LocalAuthorityService(CrmServiceArgs args) : base(args)
        {
            _localAuthorityRepository = CrmRepositoriesFactory.Get<ILocalAuthorityRepository>();
        }

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
        #endregion
    }
}
