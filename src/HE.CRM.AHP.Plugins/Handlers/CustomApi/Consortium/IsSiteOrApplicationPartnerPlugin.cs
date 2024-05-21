using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using static HE.CRM.Common.Enums.AccountEnum;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class IsSiteOrApplicationPartnerPlugin : CrmActionHandlerBase<invln_issiteorapplicationpartnerRequest, DataverseContext>
    {
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_issiteorapplicationpartnerRequest.Fields.invln_consortiumid);
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_issiteorapplicationpartnerRequest.Fields.invln_organizationid);

        private readonly IAhpApplicationRepository _applicationRepository;

        private readonly ISiteRepository _siteRepository;

        public IsSiteOrApplicationPartnerPlugin(IAhpApplicationRepository ahpApplicationRepository,
            ISiteRepository siteRepository)
        {
            _applicationRepository = ahpApplicationRepository;
            _siteRepository = siteRepository;
        }

        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(ConsortiumId) && !string.IsNullOrEmpty(OrganizationId);
        }

        public override void DoWork()
        {
            var accountId = new Guid(OrganizationId);
            var isApplicationPartner = AppPartner(accountId);
            var isSitePartner = SitePartner(accountId);
            if (isSitePartner)
            {
                ExecutionData.SetOutputParameter(invln_issiteorapplicationpartnerResponse
                    .Fields.invln_consortiumpartnerstatus, (int)PartnerType.SitePartner);
            }
            else if (isApplicationPartner)
            {
                ExecutionData.SetOutputParameter(invln_issiteorapplicationpartnerResponse
                    .Fields.invln_consortiumpartnerstatus, (int)PartnerType.ApplicationPartner);
            }
            else
            {
                ExecutionData.SetOutputParameter(invln_issiteorapplicationpartnerResponse
                    .Fields.invln_consortiumpartnerstatus, (int)PartnerType.None);
            }

        }

        private bool SitePartner(Guid accountId)
        {
            var applications = _applicationRepository.GetByConsortiumId(new Guid(ConsortiumId));
            return applications.Any(x => x.invln_DevelopingPartner != null && x.invln_DevelopingPartner.Id == accountId);
        }

        private bool AppPartner(Guid accountId)
        {
            var sites = _siteRepository.GetbyConsortiumId(new Guid(ConsortiumId));
            return sites.Any(x => x.invln_developingpartner != null && x.invln_developingpartner.Id == accountId);
        }
    }
}
