using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.DeliveryPhase;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.DeliveryPhase
{

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        invln_DeliveryPhase.EntityLogicalName,
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        invln_DeliveryPhase.Fields.StatusCode + "," + invln_DeliveryPhase.Fields.invln_NoofHomes,
        "HE.CRM.AHP.Plugins.Plugins.DeliveryPhase.PreUpdateDeliveryPhase : PreUpdate of Delivery Phase",
        1,
        IsolationModeEnum.Sandbox,
        Id = "acb6edf2-59bf-42fd-8416-74ba9b55e51a",
        Image1Name = "PreImage",
        Image1Attributes = invln_DeliveryPhase.Fields.StatusCode + "," + invln_DeliveryPhase.Fields.invln_Application + "," +
        invln_DeliveryPhase.Fields.ModifiedBy + "," + invln_DeliveryPhase.Fields.invln_NoofHomes,
        Image1Type = ImageTypeEnum.PreImage)]
    public class PreUpdateDeliveryPhase : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<AcceptOrRejectPaymentProportion>());
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeNumberOfHomes>());
        }
    }
}