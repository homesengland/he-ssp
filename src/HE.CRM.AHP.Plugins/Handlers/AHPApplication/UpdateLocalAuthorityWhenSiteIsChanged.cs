using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class UpdateLocalAuthorityWhenSiteIsChanged : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        private readonly IAhgLocalAuthorityRepository _localAuthorityRepository;

        private readonly IAhpApplicationRepository _ahpApplicationRepository;

        public UpdateLocalAuthorityWhenSiteIsChanged(IAhgLocalAuthorityRepository localAuthorityRepository, IAhpApplicationRepository ahpApplicationRepository)
        {
            _localAuthorityRepository = localAuthorityRepository;
            _ahpApplicationRepository = ahpApplicationRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_scheme.Fields.invln_Site) && CurrentState.invln_Site != null;
        }

        public override void DoWork()
        {
            var ahpLocalAuthority = _localAuthorityRepository.GetAhpLocalAuthoritiesReletedToSite(CurrentState.invln_Site.Id);
            _ahpApplicationRepository.Assign(new invln_scheme() { Id = CurrentState.Id }, ahpLocalAuthority.invln_GrowthManager);
        }
    }
}
