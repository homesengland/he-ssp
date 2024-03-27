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
            return ValueChanged(invln_scheme.Fields.invln_Site) && ExecutionData.Target.invln_Site != null;
        }

        public override void DoWork()
        {
            this.TracingService.Trace($"{ExecutionData.Target.invln_Site.Id}");
            var ahpLocalAuthority = _localAuthorityRepository.GetAhpLocalAuthoritiesReletedToSite(ExecutionData.Target.invln_Site.Id);
            this.TracingService.Trace($"Grow Manager");
            if (ahpLocalAuthority.invln_GrowthManager == null)
                return;
            _ahpApplicationRepository.Assign(new invln_scheme() { Id = CurrentState.Id }, ahpLocalAuthority.invln_GrowthManager);
        }
    }
}
