using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_invitecontacttojoinexistingorganisation",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.InviteContactToJoinExistingOrganisationPlugin: invln_invitecontacttojoinexistingorganisation",
    1,
    IsolationModeEnum.Sandbox,
    Id = "f3cc5918-f130-457f-bfa4-043f7f1f44f6")]
    public class InviteContactToJoinExistingOrganisationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public InviteContactToJoinExistingOrganisationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<InviteContactToJoinExistingOrganisationHandler>());
        }
        #endregion
    }
}
