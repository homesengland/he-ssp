using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getloanapplicationsforaccountandcontact",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetInvestmentsLoansForAccountAndContactPlugin: invln_getloanapplicationsforaccountandcontact",
    1,
    IsolationModeEnum.Sandbox,
    Id = "1af27057-ec49-4ed0-a1ef-1a8fcf254372")]
    public class GetInvestmentsLoansForAccountAndContactPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetInvestmentsLoansForAccountAndContactPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetInvestmentsLoansForAccountAndContactHandler>());
        }
        #endregion
    }
}
