using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.HomeType
{
    public class CalculatePercentageValueOfNDSSStandardHandler : CrmEntityHandlerBase<invln_HomeType, DataverseContext>
    {
        private readonly INdssRepository _ndssRepository;

        public CalculatePercentageValueOfNDSSStandardHandler(INdssRepository ndssRepository)
        {
            this._ndssRepository = ndssRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace("CalculatePercentageValueOfNDSSStandard - Can Work");
            return ValueChanged(invln_HomeType.Fields.invln_numberofbedrooms) || ValueChanged(invln_HomeType.Fields.invln_maxoccupancy) || ValueChanged(invln_HomeType.Fields.invln_numberofstoreys);
        }

        public override void DoWork()
        {
            TracingService.Trace("CalculatePercentageValueOfNDSSStandard - Do Work");
            if (CurrentState.invln_numberofbedrooms == null ||
                CurrentState.invln_maxoccupancy == null ||
                CurrentState.invln_numberofstoreys == null)
            {
                TracingService.Trace($"One of: {CurrentState.invln_numberofbedrooms}, {CurrentState.invln_maxoccupancy}, {CurrentState.invln_numberofstoreys} is null calculation is not possible");
                return;
            }

            var concatenatevalue = $"{CurrentState.invln_numberofbedrooms}{CurrentState.invln_maxoccupancy}{CurrentState.invln_numberofstoreys}";
            var nddsStandard = _ndssRepository.GetByAttribute(invln_ndss.Fields.invln_StandardNumber, int.Parse(concatenatevalue), new string[] { invln_ndss.Fields.invln_StandardNumber, invln_ndss.Fields.invln_Standard }).FirstOrDefault();
            if (nddsStandard == null)
            {
                TracingService.Trace($"Cannot find entity {invln_ndss.EntityLogicalName} with value {concatenatevalue} in field {invln_ndss.Fields.invln_StandardNumber}");
                throw new InvalidPluginExecutionException("Home type not covered by NDSS");
            }
            TracingService.Trace($"Calculate value in {invln_HomeType.Fields.invln_PercentageValueofNDSSStandard}");

            var pecentageValueOfNdssStandard = (decimal)(nddsStandard.invln_Standard.Value / CurrentState.invln_floorarea * 100);
            ExecutionData.Target.invln_PercentageValueofNDSSStandard = pecentageValueOfNdssStandard;
        }
    }
}
