using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getsinglefrontdoorproject",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetSingleFrontDoorProjectForAccountAndContactPlugin: invln_getsinglefrontdoorproject",
    1,
    IsolationModeEnum.Sandbox,
    Id = "10E74A4B-DD1F-4033-B29D-1582BD4D1FC2")] //done

    public class GetSingleFrontDoorProjectForAccountAndContactPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetSingleFrontDoorProjectForAccountAndContactPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleFrontDoorProjectForAccountAndContactHandler>());
        }
        #endregion
    }
}
