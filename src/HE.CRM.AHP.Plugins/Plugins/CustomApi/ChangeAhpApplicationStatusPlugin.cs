using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_changeahpapplicationstatus",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.ChangeAhpApplicationStatus: invln_changeahpapplicationstatus",
    1,
    IsolationModeEnum.Sandbox,
    Id = "792e1af3-2889-4d72-b12e-c4c757605603")]
    public class ChangeAhpApplicationStatusPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public ChangeAhpApplicationStatusPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeAhpApplicationHandler>());
        }
        #endregion
    }
}
