using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_sendremindertofinalisadraftapplication",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.AHP.Plugins.Plugins.CustomApi.SendReminderEmailForFinaliseDraftApplicationPlugin: invln_sendremindertofinalisadraftapplication",
    1,
    IsolationModeEnum.Sandbox,
    Id = "5edd2f4c-c1ff-4ba8-93d7-cd6d9e2b9bb1")]
    public class SendReminderEmailForFinaliseDraftApplicationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendReminderEmailForFinaliseDraftApplicationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendReminderEmailForFinaliseDraftApplicationHandler>());
        }
        #endregion
    }
}
