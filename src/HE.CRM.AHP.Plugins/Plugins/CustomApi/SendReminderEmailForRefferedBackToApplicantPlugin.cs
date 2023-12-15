using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sendreminderemailforrefferedbacktoapplicant",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.SendReminderEmailForRefferedBackToApplicantPlugin: invln_sendreminderemailforrefferedbacktoapplicant",
    1,
    IsolationModeEnum.Sandbox,
    Id = "b1c4bb9d-02f0-4042-ae79-7ed048e456be")]
    public class SendReminderEmailForRefferedBackToApplicantPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendReminderEmailForRefferedBackToApplicantPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendReminderEmailForRefferedBackToApplicantHandler>());
        }
        #endregion
    }
}
