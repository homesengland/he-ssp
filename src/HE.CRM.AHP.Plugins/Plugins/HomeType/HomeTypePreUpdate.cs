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
            invln_HomeType.Fields.invln_numberofbedrooms + "," +
            invln_HomeType.Fields.invln_maxoccupancy + "," +
            invln_HomeType.Fields.invln_numberofstoreys + "," +
            invln_HomeType.Fields.invln_whichndssstandardshavebeenmet,
        "HE.CRM.Plugins.Plugins.HomeType.HomeTypePreUpdate: Update of Home Type",
        1,
        IsolationModeEnum.Sandbox,
        Id = "51ccac9e-baff-4df7-bc70-ae13d9749b35",
        Image1Name = "PreImage",
        Image1Attributes = invln_HomeType.Fields.invln_numberofbedrooms + "," +
                        invln_HomeType.Fields.invln_maxoccupancy + "," +
                        invln_HomeType.Fields.invln_numberofstoreys + "," +
                        invln_HomeType.Fields.invln_whichndssstandardshavebeenmet + "," +
                        invln_HomeType.Fields.invln_floorarea,
        Image1Type = ImageTypeEnum.PreImage
    )]
    public class HomeTypePreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CalculatePercentageValueOfNDSSStandardHandler>());
        }
    }
}
