using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.Site;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Site
{
    public class GetMultipleSitesHandler : CrmActionHandlerBase<invln_getmultiplesitesRequest, DataverseContext>
    {
        #region Fields
        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_fieldstoretrieve);
        private string Paging => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_pagingrequest);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_externalcontactid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_getmultiplesitesRequest.Fields.invln_accountid);

        #endregion


        public override bool CanWork()
        {
            return !string.IsNullOrWhiteSpace(Paging);
        }

        public override void DoWork()
        {
            TracingService.Trace("GetMultipleSitesHandler");
            var paging = JsonSerializer.Deserialize<PagingRequestDto>(Paging);
            var result = CrmServicesFactory.Get<ISiteService>().GetMultiple(paging, FieldsToRetrieve, externalContactId, accountId);
            if (result != null)
            {
                ExecutionData.SetOutputParameter(invln_getmultiplesitesResponse.Fields.invln_sites, JsonSerializer.Serialize(result));
            }
        }
    }
}
