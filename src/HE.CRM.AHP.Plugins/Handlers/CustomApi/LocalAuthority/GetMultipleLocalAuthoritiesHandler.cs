using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Services.LocalAuthority;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.LocalAuthority
{
    public class GetMultipleLocalAuthoritiesHandler : CrmActionHandlerBase<invln_getmultiplelocalauthoritiesRequest, DataverseContext>
    {
        private string Paging => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesRequest.Fields.invln_pagingrequest);

        private string FieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesRequest.Fields.invln_fieldstoretrieve);

        private string SearchPhrase => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesRequest.Fields.invln_searchphrase);

        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var paging = JsonSerializer.Deserialize<PagingRequestDto>(Paging);
            if (paging != null)
            {
                var result = CrmServicesFactory.Get<ILocalAuthorityService>().Get(paging, SearchPhrase, FieldsToRetrieve);
                if (result != null)
                {
                    var serialized = JsonSerializer.Serialize(result);
                    ExecutionData.SetOutputParameter(invln_getmultiplelocalauthoritiesResponse.Fields.invln_localauthorities, serialized);
                }
            }
        }
    }
}
