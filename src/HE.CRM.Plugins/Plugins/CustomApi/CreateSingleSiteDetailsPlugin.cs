using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_createsinglesitedetail",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.CreateSingleSiteDetailsPlugin: invln_createsinglesitedetail",
    1,
    IsolationModeEnum.Sandbox,
    Id = "00c02bc5-8fc9-4be2-8347-f6fef376dab8")]
    public class CreateSingleSiteDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public CreateSingleSiteDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateSingleSiteDetailsHandler>());
        }
    }
}
