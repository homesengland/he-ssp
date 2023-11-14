using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_scheme.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "invln_name",
      "HE.CRM.Plugins.Plugins.Account.CheckIfApplicationWithNewNameExistsPlugin: Create of AHP Application",
      1,
      IsolationModeEnum.Sandbox,
      Id = "e495107d-4b4c-4275-be70-7ba40d364fa8")]
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_scheme.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "invln_name",
      "HE.CRM.Plugins.Plugins.Account.CheckIfApplicationWithNewNameExistsPlugin: Update of AHP Application",
      1,
      IsolationModeEnum.Sandbox,
      Id = "631d2ec4-506f-4714-bd95-821e4940eef5")]
    internal class CheckIfApplicationWithNewNameExistsPlugin
    {
    }
}
