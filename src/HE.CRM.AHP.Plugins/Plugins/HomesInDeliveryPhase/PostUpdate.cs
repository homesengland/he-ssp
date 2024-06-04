using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.HomesInDeliveryPhases;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.HomesInDeliveryPhase
{
    [CrmPluginRegistration(
        MessageNameEnum.Update,
        invln_HomeType.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        invln_homesindeliveryphase.Fields.invln_numberofhomes,
        "HE.CRM.Plugins.Plugins.HomeType.HomesInDeliveryPhase: Update of Homes In Delivery Phase",
        1,
        IsolationModeEnum.Sandbox,
        Id = "c27cc842-497e-4ba7-bcd3-b853296573ba",
        Image1Name = "PreImage",
        Image1Attributes = invln_homesindeliveryphase.Fields.invln_numberofhomes + "," +
        invln_homesindeliveryphase.Fields.invln_homesindeliveryphaseId + "," +
        invln_homesindeliveryphase.Fields.invln_deliveryphaselookup
        ,
        Image1Type = ImageTypeEnum.PreImage
    )]
    public class PostUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CalculateNumberOfHouseForDeliveryPhase>());
        }
    }
}
