using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_searchlocalauthority",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.SearchLocalAuthority: invln_searchlocalauthority",
    1,
    IsolationModeEnum.Sandbox,
    Id = "31c1a53b-31d8-4606-a9b7-a4946b162899")]
    public class SearchLocalAuthorityPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SearchLocalAuthorityPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SearchLocalAuthorityHandler>());
        }
        #endregion
    }
}
