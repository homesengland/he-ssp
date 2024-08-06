using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.Claims;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.Claim
{
    [CrmPluginRegistration(
    MessageNameEnum.Update,
    invln_Claim.EntityLogicalName,
    StageEnum.PreOperation,
    ExecutionModeEnum.Synchronous,
    filteringAttributes: "statuscode",
    stepName: "HE.CRM.AHP.Plugins.Plugins.Claim.PreUpdate: Update of invln_Claim",
    executionOrder: 1,
    IsolationModeEnum.Sandbox,
    Id = "3B59636F-EBA5-407E-B1BD-CDC82067A9A5",
    Image1Type = ImageTypeEnum.PreImage,
    Image1Name = "PreImage",
    Image1Attributes = "statuscode"
)]
    public class PreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeClaimExternalStatusOnClaimInternalStatusChangeHandler>());
        }
    }
}

