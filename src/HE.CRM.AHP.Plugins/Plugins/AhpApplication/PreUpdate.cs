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
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        filteringAttributes: "statuscode",
        stepName: "HE.CRM.AHP.Plugins.Plugins.AhpApplication.PreUpdate: Update of invln_scheme",
        executionOrder: 5,
        IsolationModeEnum.Sandbox,
        Id = "a38aaf6b-7bff-4c2d-9a36-5715d21d081d",
        Image1Type = ImageTypeEnum.PreImage,
        Image1Name = "PreImage",
        Image1Attributes = "statuscode"
    )]
    public class PreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<PopulateFieldsForApplicationReportHandler>());
        }
    }
}
