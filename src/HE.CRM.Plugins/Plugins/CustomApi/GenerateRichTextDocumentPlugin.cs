using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_generaterichtextdocument",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.SendInvestmentsLoanDataToCrmPlugin: invln_generaterichtextdocument",
    1,
    IsolationModeEnum.Sandbox,
    Id = "AC1541B7-EA6A-46E1-A836-2C57C310CB33")]
    public class GenerateRichTextDocumentPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GenerateRichTextDocumentPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GenerateRichTextDocumentHandler>());
        }
    }
}
