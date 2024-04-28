using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
    "invln_getconsortiums",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Consortium.GetConsortiumsByOrganisationPlugin: invln_getconsortiums",
    1,
    IsolationModeEnum.Sandbox,
    Id = "ebd32009-dfcf-4b93-b8d1-b071255c0273")]
    public class GetConsortiumsByOrganisationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetConsortiumsByOrganisationHandler>());
        }
    }
}
