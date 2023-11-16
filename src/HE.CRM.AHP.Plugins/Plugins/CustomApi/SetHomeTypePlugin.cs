using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sethometype",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.SetHomeTypePlugin: invln_sethometype",
    1,
    IsolationModeEnum.Sandbox,
    Id = "a2db4c40-f557-45aa-89fd-d15ba648d090")]
    public class SetHomeTypePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetHomeTypePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetHomeTypeHandler>());
        }
        #endregion
    }
}
