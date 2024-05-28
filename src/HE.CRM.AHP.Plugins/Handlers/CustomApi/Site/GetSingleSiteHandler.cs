using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class GetSingleSiteHandler : CrmActionHandlerBase<invln_getsinglesiteRequest, DataverseContext>
    {
        private string SiteId => ExecutionData.GetInputParameter<string>(invln_getsinglesiteRequest.Fields.invln_siteid);
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglesiteRequest.Fields.invln_fieldstoretrieve);
        private string ExternalContactId => ExecutionData.GetInputParameter<string>(invln_getsinglesiteRequest.Fields.invln_externalcontactid);
        private string AccountId => ExecutionData.GetInputParameter<string>(invln_getsinglesiteRequest.Fields.invln_accountid);

        public override bool CanWork()
        {
            TracingService.Trace("Input Parameters:");
            TracingService.Trace($"SiteId: {SiteId}");
            TracingService.Trace($"FieldsToRetrieve: {FieldsToRetrieve}");
            TracingService.Trace($"ExternalContactId: {ExternalContactId}");
            TracingService.Trace($"AccountId: {AccountId}");

            return !string.IsNullOrWhiteSpace(SiteId);
        }

        public override void DoWork()
        {
            TracingService.Trace("GetSingleSiteHandler");
            var result = CrmServicesFactory.Get<ISiteService>().GetSingle(SiteId, FieldsToRetrieve, ExternalContactId, AccountId);
            if (result != null)
            {
                var serialized = JsonSerializer.Serialize(result);
                TracingService.Trace($"output: {serialized}");
                ExecutionData.SetOutputParameter(invln_getsinglesiteResponse.Fields.invln_site, serialized);
            }
        }
    }
}
