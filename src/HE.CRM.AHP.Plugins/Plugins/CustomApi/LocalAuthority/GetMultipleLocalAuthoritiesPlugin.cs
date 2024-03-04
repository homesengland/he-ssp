using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.LocalAuthority;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.LocalAuthority
{
    [CrmPluginRegistration(
        "invln_getmultiplelocalauthorities",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.LocalAuthority.GetMultipleLocalAuthoritiesPlugin: invln_getmultiplelocalauthorities",
        1,
        IsolationModeEnum.Sandbox,
        Id = "ED2F98B8-5822-4070-B9EC-535DB6AFB84E")]
    public class GetMultipleLocalAuthoritiesPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetMultipleLocalAuthoritiesPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleLocalAuthoritiesHandler>());
        }
    }
}
