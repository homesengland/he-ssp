using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sendinvitationtocontacttojoinorganizationbypowerapps",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.SentInviteContactToJoinOrganisationPowerAppsPlugin: invln_sendinvitationtocontacttojoinorganizationbypowerapps",
    1,
    IsolationModeEnum.Sandbox,
    Id = "0F174457-0F30-412E-BCD7-3CEBA44FC5A9")]



    public class SentInviteContactToJoinOrganisationPowerAppsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public SentInviteContactToJoinOrganisationPowerAppsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SentInviteContactToJoinOrganisationPowerAppsHandler>());
        }
    }
}
