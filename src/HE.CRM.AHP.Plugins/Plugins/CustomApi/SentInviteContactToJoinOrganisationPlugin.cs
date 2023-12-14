using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sendinvitecontacttojoinorganisation",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.SentInviteContactToJoinOrganisationPlugin: invln_setahpapplication",
    1,
    IsolationModeEnum.Sandbox,
    Id = "8267ED67-14F3-4EEF-9953-9260756448F9")]
    public class SentInviteContactToJoinOrganisationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SentInviteContactToJoinOrganisationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SentInviteContactToJoinOrganisationHandler>());
        }
        #endregion
    }
}
