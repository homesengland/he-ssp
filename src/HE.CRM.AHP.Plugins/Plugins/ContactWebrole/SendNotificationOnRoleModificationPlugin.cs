using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.ContactWebrole;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.ContactWebrole
{
    [CrmPluginRegistration(
     MessageNameEnum.Create,
     invln_contactwebrole.EntityLogicalName,
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.AHP.Plugins.Plugins.ContactWebrole.SendNotificationOnRoleModificationPlugin: Create of Ahp Status Change",
     1,
     IsolationModeEnum.Sandbox,
     Id = "4AB83235-B50C-4946-B7E6-DE672BEFCD67")]

    [CrmPluginRegistration(
     MessageNameEnum.Update,
     invln_contactwebrole.EntityLogicalName,
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.AHP.Plugins.Plugins.ContactWebrole.SendNotificationOnRoleModificationPlugin: Update of Ahp Status Change",
     1,
     IsolationModeEnum.Sandbox,
     Id = "F61ADC23-DF63-4D02-8F86-6E4B28A7233D")]

    [CrmPluginRegistration(
     MessageNameEnum.Delete,
     invln_contactwebrole.EntityLogicalName,
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.AHP.Plugins.Plugins.ContactWebrole.SendNotificationOnRoleModificationPlugin: Delete of Ahp Status Change",
     1,
     IsolationModeEnum.Sandbox,
     Id = "B0FB7ECB-6E4B-4031-9983-B97CF32D1144")]
    public class SendNotificationOnRoleModificationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendNotificationOnRoleModificationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendNotificationOnRoleModificationHandler>());
        }
        #endregion
    }
}
