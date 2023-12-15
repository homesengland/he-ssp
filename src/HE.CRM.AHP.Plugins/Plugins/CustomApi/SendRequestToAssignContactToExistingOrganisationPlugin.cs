using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sendrequesttoassigncontacttoexistingorganisation",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.SendRequestToAssignContactToExistingOrganisationPlugin: invln_sendrequesttoassigncontacttoexistingorganisation",
    1,
    IsolationModeEnum.Sandbox,
    Id = "a657d81e-9a3a-4f14-b2f6-2b5cf649623d")]
    public class SendRequestToAssignContactToExistingOrganisationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendRequestToAssignContactToExistingOrganisationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendRequestToAssignContactToExistingOrganisationHandler>());
        }
        #endregion
    }
}
