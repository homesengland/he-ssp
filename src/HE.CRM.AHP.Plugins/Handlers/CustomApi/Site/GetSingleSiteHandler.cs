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

        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var result = CrmServicesFactory.Get<ISiteService>().GetById(SiteId, FieldsToRetrieve);
            if (result != null)
            {
                var serialized = JsonSerializer.Serialize(result);
                ExecutionData.SetOutputParameter(invln_getsinglesiteResponse.Fields.invln_site, serialized);
            }
        }
    }
}
