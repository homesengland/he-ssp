using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class GetMultipleSitesHandler : CrmActionHandlerBase<invln_getmultiplesitesRequest, DataverseContext>
    {
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_fieldstoretrieve);

        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var sites = CrmServicesFactory.Get<ISiteService>().GetSites(FieldsToRetrieve);
            if (sites != null)
            {
                var serialized = JsonSerializer.Serialize(sites);
                ExecutionData.SetOutputParameter(invln_getmultiplesitesResponse.Fields.invln_sites, serialized);
            }
        }
    }
}
