using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getorganisationchangedetails",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetOrganisationChangeDetailsPlugin: invln_getorganisationchangedetails",
    1,
    IsolationModeEnum.Sandbox,
    Id = "f962b813-eacb-4fe9-ae96-92ad15fad307")]
    public class GetOrganisationChangeDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetOrganisationChangeDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetOrganisationChangeDetailsHandler>());
        }
        #endregion
    }
}
