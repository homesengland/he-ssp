using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.Programme;

namespace HE.CRM.AHP.Plugins.Plugins.Programme
{
    [CrmPluginRegistration(
  MessageNameEnum.Create,
  invln_programme.EntityLogicalName,
  StageEnum.PreValidation,
  ExecutionModeEnum.Synchronous,
  "",
  "HE.CRM.Plugins.Plugins.Programme.PreValidation: Create of Programme",
  1,
  IsolationModeEnum.Sandbox,
  Id = "f937f191-d0aa-4764-9a6e-f8dc6dd8a1ce")]
    public class PreValidation : PluginBase<DataverseContext>, IPlugin
    {
        public PreValidation(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ProgrammeDuplicateDetectionHandler>());
        }
    }
}
