using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class SetSiteHandler : CrmActionHandlerBase<invln_setsiteRequest, DataverseContext>
    {
        private string FieldsToSave => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_fieldstoset);
        private string Site => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_site);

        private string SiteId => ExecutionData.GetInputParameter<string>(invln_setsiteRequest.Fields.invln_siteid);

        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var site = JsonSerializer.Deserialize<SiteDto>(Site);
            if (site != null)
            {
                var id = CrmServicesFactory.Get<ISiteService>().Save(SiteId, site, FieldsToSave);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ExecutionData.SetOutputParameter(invln_setsiteResponse.Fields.invln_siteid, id);
                }
            }
        }
    }
}
