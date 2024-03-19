using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.Site
{
    public class UpdateLocalAuthorityOnSiteHandler : CrmEntityHandlerBase<invln_Sites, DataverseContext>
    {
        private readonly IAhpApplicationRepository _ahpApplicationRepository;

        private readonly IAhgLocalAuthorityRepository _localAuthorityRepository;

        public UpdateLocalAuthorityOnSiteHandler(IAhpApplicationRepository ahpApplicationRepository,
                                                 IAhgLocalAuthorityRepository localAuthorityRepository)
        {
            _ahpApplicationRepository = ahpApplicationRepository;
            _localAuthorityRepository = localAuthorityRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_Sites.Fields.invln_LocalAuthority) && CurrentState.invln_LocalAuthority != null;
        }

        public override void DoWork()
        {
            var localAuthority = _localAuthorityRepository.GetById(CurrentState.invln_LocalAuthority,
                                                                    invln_AHGLocalAuthorities.Fields.invln_GrowthManager);

            var ahpApplications = _ahpApplicationRepository.GetByAttribute(invln_scheme.Fields.invln_Site, CurrentState.Id).ToList();
            foreach (var ahpApplication in ahpApplications)
            {
                _ahpApplicationRepository.Update(new invln_scheme()
                {
                    Id = ahpApplication.Id,
                    invln_LocalAuthority = CurrentState.invln_LocalAuthority,
                });
                _ahpApplicationRepository.Assign(new invln_scheme() { Id = CurrentState.Id }, localAuthority.invln_GrowthManager);
            }
        }
    }
}
