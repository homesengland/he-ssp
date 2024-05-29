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
        "invln_actualacquisitioncost, invln_expectedacquisitioncost, invln_fundingrequired, statuscode, invln_noofhomes, invln_expectedoncosts, invln_site, invln_tenure, invln_expectedonworks",
        "HE.CRM.AHP.Plugins.Plugins.AhpApplication.PostUpdate: Update of invln_scheme",
        1,
        IsolationModeEnum.Sandbox,
        Id = "ffe50e48-f484-4cca-8605-76064e680f2f",
        Image1Name = "PreImage",
        Image1Attributes = "invln_actualacquisitioncost, invln_expectedacquisitioncost, invln_fundingrequired, statuscode, invln_noofhomes, invln_expectedoncosts, invln_site, invln_tenure, invln_expectedonworks",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Name = "PostImage",
        Image2Attributes = "invln_actualacquisitioncost, invln_expectedacquisitioncost, invln_fundingrequired, statuscode, invln_noofhomes, invln_expectedoncosts, invln_site, invln_tenure, invln_expectedonworks",
        Image2Type = ImageTypeEnum.PostImage)]
    public class PostUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GrantCalculateHandler>());
        }
    }
}
