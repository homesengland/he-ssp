using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getsinglehometype",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetSingleHomeTypePlugin: invln_getsinglehometype",
    1,
    IsolationModeEnum.Sandbox,
    Id = "9db5fa1f-7b10-41bd-993a-caa2712f94f3")]
    public class GetSingleHomeTypePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetSingleHomeTypePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleHomeTypeHandler>());
        }
        #endregion
    }
}
