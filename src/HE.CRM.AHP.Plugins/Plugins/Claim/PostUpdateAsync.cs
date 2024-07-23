using System.Collections.Generic;
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
        StageEnum.PostOperation,
        ExecutionModeEnum.Asynchronous,
        "statuscode",
        "HE.CRM.AHP.Plugins.Plugins.Claim.PostUpdateAsync: Update of invln_claim",
        1,
        IsolationModeEnum.Sandbox,
        Id = "d12050e0-eb42-4dba-98f7-84610da60d63",
        Image1Name = "PreImage",
        Image1Attributes = "statuscode",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Name = "PostImage",
        Image2Attributes = "invln_allocation, invln_application",
        Image2Type = ImageTypeEnum.PostImage
    )]
    public sealed class PostUpdateAsync : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GrantDetailsCalculateHandler>());
        }
    }
}
