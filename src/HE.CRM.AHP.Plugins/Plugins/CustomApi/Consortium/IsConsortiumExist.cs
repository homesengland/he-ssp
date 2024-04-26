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
    "invln_IsConsortiumExistForProgrammeAndOrganisation",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Consortium.IsConsortiumExist: invln_IsConsortiumExistForProgrammeAndOrganisation",
    1,
    IsolationModeEnum.Sandbox,
    Id = "4a611f96-8b9b-477d-870f-be0bef065b52")]
    public class IsConsortiumExist : PluginBase<DataverseContext>, IPlugin
    {
        public IsConsortiumExist(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<IsConsortiumExistForProgrammeAndOrganisation>());
        }
    }
}
