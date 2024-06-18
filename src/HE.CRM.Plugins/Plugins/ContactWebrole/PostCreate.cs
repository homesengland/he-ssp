using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ContactWebrole;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.ContactWebrole
{
    [CrmPluginRegistration(
        MessageNameEnum.Create,
        invln_contactwebrole.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "invln_accountid, invln_contactid",
        "HE.CRM.Plugins.Plugins.ContactWebrole.PostCreate: Create of invln_contactwebrole",
        1,
        IsolationModeEnum.Sandbox,
        Id = "2acd41fe-104f-4863-bcde-9064f2252c34")]
    public class PostCreate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<PopulateParentCustomerOnContactHandler>());
        }
    }
}
