using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
      "invln_getahpapplicationdocumentlocation",
      "none",
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.CustomApi.GetAhpApplicationFileLocationPlugin: invln_getahpapplicationdocumentlocation",
      1,
      IsolationModeEnum.Sandbox,
      Id = "0518ff2c-27b3-4335-bbd4-13a448f55be2")]
    public class GetAhpApplicationFileLocationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetAhpApplicationFileLocationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAhpApplicationFileLocationHandler>());
        }
        #endregion
    }
}
