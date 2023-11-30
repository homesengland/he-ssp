using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;

namespace HE.CRM.AHP.Plugins.Plugins.AhpApplication
{
    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_scheme.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.AhpApplication.CreateDocumentLocationOnCreatePlugin: Create of AHP Application",
      1,
      IsolationModeEnum.Sandbox,
      Id = "07040b59-157a-464d-9d0f-7fbce9927eaf")]
    public class CreateDocumentLocationOnCreatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CreateDocumentLocationOnCreatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateDocumentLocationOnCreateHandler>());
        }
        #endregion
    }
}
