using System.Text.Json;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contacts;
using HE.CRM.Plugins.Services.SiteDetails;

namespace HE.CRM.Plugins.Handlers.CustomApi
{
    public class GetSingleSiteDetailsHandler : CrmActionHandlerBase<invln_getsinglesitedetailsRequest, DataverseContext>
    {
        #region Fields

        private string siteDetailsId => ExecutionData.GetInputParameter<string>(invln_getsinglesitedetailsRequest.Fields.invln_sitedetailsid);
        private string externalContactId => ExecutionData.GetInputParameter<string>(invln_getsinglesitedetailsRequest.Fields.invln_externalcontactid);
        private string accountId => ExecutionData.GetInputParameter<string>(invln_getsinglesitedetailsRequest.Fields.invln_accountid);
        private string fieldsToRetrieve => ExecutionData.GetInputParameter<string>(invln_getsinglesitedetailsRequest.Fields.invln_fieldstoretrieve);
        private string useHeTablesFromPortal => ExecutionData.GetInputParameter<string>(invln_getsinglesitedetailsRequest.Fields.invln_usehetables);

        #endregion

        #region Base Methods Overrides
        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(siteDetailsId) && !string.IsNullOrEmpty(externalContactId) && !string.IsNullOrEmpty(accountId);
        }

        public override void DoWork()
        {
            this.TracingService.Trace("GetSingleSiteDetail");
            var useHeTables = !string.IsNullOrEmpty(useHeTablesFromPortal);
            var siteDetailsSerialized = CrmServicesFactory.Get<ISiteDetailsService>().GetSingleSiteDetail(useHeTables, siteDetailsId, accountId, externalContactId, fieldsToRetrieve);
            if (siteDetailsSerialized != null)
            {
                ExecutionData.SetOutputParameter(invln_getsinglesitedetailsResponse.Fields.invln_sitedetail, siteDetailsSerialized);
            }
        }

        #endregion
    }
}
