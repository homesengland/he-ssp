using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.HomesInDeliveryPhases;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.HomesInDeliveryPhase
{
    [CrmPluginRegistration(
    MessageNameEnum.Create,
    invln_homesindeliveryphase.EntityLogicalName,
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    invln_homesindeliveryphase.Fields.invln_numberofhomes,
    "HE.CRM.Plugins.Plugins.HomeType.HomesInDeliveryPhase: Create of HomesIn Delivery Phase",
    1,
    IsolationModeEnum.Sandbox,
    Id = "c59106d1-fd68-4bd5-a4b6-0b441059d8ce")]
    public class PostCreate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CalculateNumberOfHouseForDeliveryPhase>());
        }
    }
}
