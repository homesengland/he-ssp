using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LocalAuthority;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Handlers.CustomApi.LocalAuthority
{
    public class GetMultipleLocalAuthoritiesForModuleHandler : CrmActionHandlerBase<invln_getmultiplelocalauthoritiesformoduleRequest, DataverseContext>
    {
        #region Fields
        private string pagingRequest => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_pagingrequest);
        private string searchPhrase => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_searchphrase);
        private string isLoan => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_isloan);
        private string isAHP => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_isahp);
        private string isLoanFD => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_isloanfd);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getmultiplelocalauthoritiesformoduleRequest.Fields.invln_usehetables);


        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            var numberOfModuleSelected = 0;

            if (bool.TryParse(isLoan, out bool loan) && loan)
            {
                numberOfModuleSelected++;
            }
            if (bool.TryParse(isAHP, out bool ahp) && ahp)
            {
                numberOfModuleSelected++;
            }
            if (bool.TryParse(isLoanFD, out bool loanFD) && loanFD)
            {
                numberOfModuleSelected++;
            }

            return !string.IsNullOrEmpty(pagingRequest) && numberOfModuleSelected == 1;
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetMultipleLocalAuthoritiesForModuleHandler");

            var moduleSelected = "";

            if (bool.TryParse(isLoan, out bool loan) && loan)
            {
                moduleSelected = "loan";
            }
            if (bool.TryParse(isAHP, out bool ahp) && ahp)
            {
                moduleSelected = "ahp";
            }
            if (bool.TryParse(isLoanFD, out bool loanFD) && loanFD)
            {
                moduleSelected = "loanFD";
            }

            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var paging = JsonSerializer.Deserialize<PagingRequestDto>(pagingRequest);
            if (paging != null)
            {
                var localAuthorities = CrmServicesFactory.Get<ILocalAuthorityService>().GetLocalAuthoritiesForModule(paging, searchPhrase, moduleSelected, useHeTables);

                this.TracingService.Trace("Send Response");
                if (localAuthorities != null)
                {
                    var localAuthoritiesSerialized = JsonSerializer.Serialize(localAuthorities);
                    ExecutionData.SetOutputParameter(invln_getmultiplelocalauthoritiesformoduleResponse.Fields.invln_localauthorities, localAuthoritiesSerialized);
                }
            }

        }
        #endregion
    }
}
