using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.AhpApplication
{
    [CrmPluginRegistration(
        MessageNameEnum.Update,
        invln_scheme.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "statuscode, invln_fundingrequired, invln_noofhomes, invln_expectedacquisitioncost, invln_actualacquisitioncost, invln_expectedoncosts, invln_expectedonworks, invln_site, invln_tenure",
        "HE.CRM.AHP.Plugins.Plugins.AhpApplication.PostUpdate: Update of invln_scheme",
        1,
        IsolationModeEnum.Sandbox,
        Id = "ffe50e48-f484-4cca-8605-76064e680f2f",
        Image1Name = "PreImage",
        Image1Attributes = "statuscode, invln_fundingrequired, invln_noofhomes, invln_expectedacquisitioncost, invln_actualacquisitioncost, invln_expectedoncosts, invln_expectedonworks, invln_site, invln_tenure",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Name = "PostImage",
        Image2Attributes = "statuscode",
        Image2Type = ImageTypeEnum.PostImage)]
    public class PostUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GrantCalculateHandler>());
        }
    }
}
