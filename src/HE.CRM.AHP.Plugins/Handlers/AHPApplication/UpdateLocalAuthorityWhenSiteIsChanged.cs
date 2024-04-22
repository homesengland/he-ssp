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
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IHeLocalAuthorityRepository _heLocalAuthorityRepository;

        public UpdateLocalAuthorityWhenSiteIsChanged(IAhpApplicationRepository ahpApplicationRepository, IHeLocalAuthorityRepository heLocalAuthorityRepository)
        {
            _ahpApplicationRepository = ahpApplicationRepository;
            _heLocalAuthorityRepository = heLocalAuthorityRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_scheme.Fields.invln_Site) && ExecutionData.Target.invln_Site != null;
        }

        public override void DoWork()
        {
            this.TracingService.Trace($"{ExecutionData.Target.invln_Site.Id}");
            var heLocalAuthority = _heLocalAuthorityRepository.GetAhpLocalAuthoritiesReletedToSite(ExecutionData.Target.invln_Site.Id);
            this.TracingService.Trace($"Grow Manager");
            if (heLocalAuthority.OwningUser == null)
            {
                return;
            }

            _ahpApplicationRepository.Assign(new invln_scheme() { Id = CurrentState.Id }, heLocalAuthority.OwningUser);
        }
    }
}
