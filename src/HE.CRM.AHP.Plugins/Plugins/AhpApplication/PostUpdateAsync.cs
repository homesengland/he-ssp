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
        ExecutionModeEnum.Asynchronous,
        "statuscode",
        "HE.CRM.AHP.Plugins.Plugins.AhpApplication.PostUpdateAsync: Update of invln_scheme",
        10,
        IsolationModeEnum.Sandbox,
        Id = "720C2ECB-9DD6-4E48-B9AB-CF94E240AB73",
        Image1Name = "PreImage",
        Image1Attributes = "statuscode",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Name = "PostImage",
        Image2Attributes = "invln_isallocation",
        Image2Type = ImageTypeEnum.PostImage)]
    public class PostUpdateAsync : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateAllocationWhenApprovedHandler>());
        }
    }
}
