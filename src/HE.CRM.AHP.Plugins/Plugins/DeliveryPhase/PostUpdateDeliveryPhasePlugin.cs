using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.DeliveryPhase;

namespace HE.CRM.AHP.Plugins.Plugins.DeliveryPhase
{
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_DeliveryPhase.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "statuscode",
       "HE.CRM.AHP.Plugins.Plugins.DeliveryPhase.PostUpdateDeliveryPhase : PostUpdate of Delivery Phase",
      1,
      IsolationModeEnum.Sandbox,
      Id = "669721D5-448B-485F-81F5-540F254F9993",
       Image1Name = "PreImage",
       Image1Attributes = "statuscode",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Name = "PostImage",
       Image2Attributes = "statuscode",
       Image2Type = ImageTypeEnum.PostImage)]


    public class PostUpdateDeliveryPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public PostUpdateDeliveryPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<PostUpdateDeliveryPhaseHandler>());
        }
        #endregion







    }
}
