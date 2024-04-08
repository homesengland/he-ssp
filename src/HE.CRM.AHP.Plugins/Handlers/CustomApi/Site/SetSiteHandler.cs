using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class SetSiteHandler : CrmActionHandlerBase<invln_setsiteRequest, DataverseContext>
    {
        #region Fields
        private string FieldsToSave => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_fieldstoset);
        private string Site => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_site);
        private string SiteId => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_siteid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_externalcontactid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_accountid);
        #endregion

        public override bool CanWork()
        {
            return !string.IsNullOrWhiteSpace(Site);
        }

        public override void DoWork()
        {
            TracingService.Trace("SetSiteHandler");
            var site = JsonSerializer.Deserialize<SiteDto>(Site);
            if (site != null)
            {
                var id = CrmServicesFactory.Get<ISiteService>().Save(SiteId, site, FieldsToSave, externalContactId, accountId);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ExecutionData.SetOutputParameter(invln_setsiteResponse.Fields.invln_siteid, id);
                }
            }
        }
    }
}
