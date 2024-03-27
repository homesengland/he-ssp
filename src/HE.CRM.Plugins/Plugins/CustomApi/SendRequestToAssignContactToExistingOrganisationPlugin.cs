using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.CustomApi
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
    Id = "89B35099-BEEA-443A-AE4B-937011D25E3D")]
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
