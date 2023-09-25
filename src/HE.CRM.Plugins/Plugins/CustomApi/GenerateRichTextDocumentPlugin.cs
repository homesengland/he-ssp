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
    "HE.CRM.Plugins.Plugins.CustomApi.GenerateRichTextDocumentPlugin: invln_generaterichtextdocument",
    1,
    IsolationModeEnum.Sandbox,
    Id = "67dbcb0a-02bb-4a6a-af3a-718b48ffb12a")]
    public class GenerateRichTextDocumentPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GenerateRichTextDocumentPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GenerateRichTextDocumentHandler>());
        }
        #endregion
    }
}
