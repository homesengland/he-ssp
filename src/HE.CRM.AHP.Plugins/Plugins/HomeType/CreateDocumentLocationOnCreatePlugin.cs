using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.HomeType;

namespace HE.CRM.AHP.Plugins.Plugins.HomeType
{
    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_HomeType.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.HomeType.CreateDocumentLocationOnCreate: Create of Home Type",
      1,
      IsolationModeEnum.Sandbox,
      Id = "e545c818-422a-4601-b339-8461af78ce52")]
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
