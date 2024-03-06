using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class GetMultipleSitesHandler : CrmActionHandlerBase<invln_getmultiplesitesRequest, DataverseContext>
    {
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_fieldstoretrieve);

        private string Paging => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_pagingrequest);

        public override bool CanWork()
        {
            return !string.IsNullOrWhiteSpace(Paging);
        }

        public override void DoWork()
        {
            var paging = JsonSerializer.Deserialize<PagingRequestDto>(Paging);
            var result = CrmServicesFactory.Get<ISiteService>().Get(paging, FieldsToRetrieve);
            if (result != null)
            {
                ExecutionData.SetOutputParameter(invln_getmultiplesitesResponse.Fields.invln_sites, JsonSerializer.Serialize(result));
            }
        }
    }
}
