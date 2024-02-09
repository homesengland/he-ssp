using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "invln_urbrequestingearlymilestonepayments",
       "HE.CRM.AHP.Plugins.Plugins.DeliveryPhase.SendNotificationUrbWantsAdditionalPaymentsForPhasePlugin: Update of Delivery Phase",
       1,
       IsolationModeEnum.Sandbox,
       Id = "E1C11C43-582D-4D3F-9B56-B1AE3B5C4E49",
       Image1Name = "PreImage",
       Image1Attributes = "invln_urbrequestingearlymilestonepayments,invln_application",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Name = "PostImage",
       Image2Attributes = "invln_urbrequestingearlymilestonepayments,invln_application",
       Image2Type = ImageTypeEnum.PostImage)]



    public class SendNotificationUrbWantsAdditionalPaymentsForPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendNotificationUrbWantsAdditionalPaymentsForPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendNotificationUrbWantsAdditionalPaymentsForPhaseHandler>());
        }
        #endregion
    }
}
