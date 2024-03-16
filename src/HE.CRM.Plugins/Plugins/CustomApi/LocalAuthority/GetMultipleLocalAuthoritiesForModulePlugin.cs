using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.LocalAuthority;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.LocalAuthority
{
    [CrmPluginRegistration(
    "invln_getmultiplelocalauthoritiesformodule",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.LocalAuthority.GetMultipleLocalAuthoritiesForModulePlugin: invln_getmultiplelocalauthoritiesformodule",
    1,
    IsolationModeEnum.Sandbox,
    Id = "B68F7971-E099-422B-820C-4936A211CF37")]

    public class GetMultipleLocalAuthoritiesForModulePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleLocalAuthoritiesForModulePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleLocalAuthoritiesForModuleHandler>());
        }
        #endregion
    }
}
