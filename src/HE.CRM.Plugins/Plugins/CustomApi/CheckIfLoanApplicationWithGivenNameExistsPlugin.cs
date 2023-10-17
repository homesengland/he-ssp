using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_changeloanapplicationexternalstatus",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.ChangeLoanApplicationExternalStatusPlugin: invln_changeloanapplicationexternalstatus",
    1,
    IsolationModeEnum.Sandbox,
    Id = "f58b6af4-7c28-4348-a2a1-d8c5fcb87d4b")]
    public class CheckIfLoanApplicationWithGivenNameExistsPlugin
    {
    }
}
