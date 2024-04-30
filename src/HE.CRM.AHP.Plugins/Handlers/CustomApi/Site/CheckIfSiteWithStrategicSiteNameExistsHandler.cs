using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class CheckIfSiteWithStrategicSiteNameExistsHandler : CrmActionHandlerBase<invln_checkifsitewithstrategicsitenameexistsRequest, DataverseContext>
    {
        private string strategicSiteName => ExecutionData.GetInputParameter<string>(invln_checkifsitewithstrategicsitenameexistsRequest.Fields.invln_sitename);
        private string organisationId => ExecutionData.GetInputParameter<string>(invln_checkifsitewithstrategicsitenameexistsRequest.Fields.invln_accountid);

        public override bool CanWork()
        {
            return !string.IsNullOrWhiteSpace(strategicSiteName) && Guid.TryParse(organisationId, out Guid organisationGuid);
        }

        public override void DoWork()
        {
            TracingService.Trace($"CheckIfSiteWithStrategicSiteNameExistsHandler");
            Guid.TryParse(organisationId, out Guid organisationGuid);
            var exist = CrmServicesFactory.Get<ISiteService>().StrategicSiteNameExists(strategicSiteName, organisationGuid);
            ExecutionData.SetOutputParameter(invln_checkifsitewithstrategicsitenameexistsResponse.Fields.invln_siteexists, exist);
        }
    }
}
