using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_gettypeofhomeslist",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetTypeOfHomesListPlugin: invln_gettypeofhomeslist",
    1,
    IsolationModeEnum.Sandbox,
    Id = "1c24ac0a-4234-4617-867e-0aa3a8ebbe9a")]
    public class GetTypeOfHomesListPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetTypeOfHomesListPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetTypeOfHomesListHandler>());
        }
        #endregion
    }
}
