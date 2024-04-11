using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.HomeType;

namespace HE.CRM.AHP.Plugins.Plugins.HomeType
{
    [CrmPluginRegistration(
  MessageNameEnum.Update,
  invln_HomeType.EntityLogicalName,
  StageEnum.PreOperation,
  ExecutionModeEnum.Synchronous,
  "invln_whichndssstandardshavebeenmet",
  "HE.CRM.Plugins.Plugins.HomeType.SetWhichNdssStandardsHaveBeenMetValuePlugin: Update of Home Type",
  1,
  IsolationModeEnum.Sandbox,
  Id = "51ccac9e-baff-4df7-bc70-ae13d9749b35")]
    public class HomeTypePreUpdate
    {
    }
}
