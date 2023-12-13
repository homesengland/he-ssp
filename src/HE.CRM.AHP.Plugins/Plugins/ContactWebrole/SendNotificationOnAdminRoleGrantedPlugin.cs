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
     "HE.CRM.AHP.Plugins.Plugins.ContactWebrole.SendNotificationOnAdminRoleGrantedPlugin: Create of Ahp Status Change",
     1,
     IsolationModeEnum.Sandbox,
     Id = "79E5FFC6-545D-4342-9189-FBE5D3B63E9E")]

    [CrmPluginRegistration(
     MessageNameEnum.Update,
     invln_contactwebrole.EntityLogicalName,
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.AHP.Plugins.Plugins.ContactWebrole.SendNotificationOnAdminRoleGrantedPlugin: Update of Ahp Status Change",
     1,
     IsolationModeEnum.Sandbox,
     Id = "B096E386-FCB1-4AAA-8AF5-8823A6475CAC")]

    public class SendNotificationOnAdminRoleGrantedPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendNotificationOnAdminRoleGrantedPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
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
