using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;
using HE.CRM.Plugins.Services.LocalAuthority;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class SearchLocalAuthorityHandler : CrmActionHandlerBase<invln_searchlocalauthorityRequest, DataverseContext>
    {
        #region Fields

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            this.TracingService.Trace("SearchLocalAuthorityHandler");
            var localAuthorities = CrmServicesFactory.Get<ILocalAuthorityService>().GetAllLocalAuthoritiesAsDto();
            this.TracingService.Trace("End custom api");
            if (localAuthorities != null)
            {
                var serializedData = JsonSerializer.Serialize(localAuthorities);
                ExecutionData.SetOutputParameter(invln_searchlocalauthorityResponse.Fields.invln_localauthorities, serializedData);
            }
        }

        #endregion
    }
}
