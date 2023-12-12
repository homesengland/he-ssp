using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
      "invln_getabsoluteahpapplicationfilelocation",
      "none",
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.CustomApi.GetAbsoluteAhpApplicationFileLocationPlugin: invln_getabsoluteahpapplicationfilelocation",
      1,
      IsolationModeEnum.Sandbox,
      Id = "05d0520c-c8b0-44cb-993f-8d74d432c1d3")]
    public class GetAbsoluteAhpApplicationFileLocationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetAbsoluteAhpApplicationFileLocationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAbsoluteAhpApplicationFileLocationHandler>());
        }
        #endregion
    }
}
