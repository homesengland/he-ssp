using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getmultipleprogramme",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetMultipleProgrammePlugin: invln_getmultipleprogramme",
    1,
    IsolationModeEnum.Sandbox,
    Id = "E8D3173A-ACB2-47E8-8E07-30215554EC0A")] //done

    public class GetMultipleProgrammePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleProgrammePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleProgrammeHandler>());
        }
        #endregion

    }
}
