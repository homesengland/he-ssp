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
        private readonly IAhpApplicationRepository _ahpApplicationRepository;

        public CalculatePercentageValueOfNDSSStandardHandler(INdssRepository ndssRepository, IAhpApplicationRepository ahpApplicationRepository)
        {
            _ndssRepository = ndssRepository;
            _ahpApplicationRepository = ahpApplicationRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace("CalculatePercentageValueOfNDSSStandard - Can Work");
            return ValueChanged(invln_HomeType.Fields.invln_numberofbedrooms)
                || ValueChanged(invln_HomeType.Fields.invln_maxoccupancy)
                || ValueChanged(invln_HomeType.Fields.invln_numberofstoreys)
                || ValueChanged(invln_HomeType.Fields.invln_floorarea);
        }

        public override void DoWork()
        {
            TracingService.Trace("CalculatePercentageValueOfNDSSStandard - Do Work");
            if (CurrentState.invln_numberofbedrooms == null ||
                CurrentState.invln_maxoccupancy == null ||
                CurrentState.invln_numberofstoreys == null ||
                CurrentState.invln_floorarea == null)

            {
                TracingService.Trace($"One of: {CurrentState.invln_numberofbedrooms}, {CurrentState.invln_maxoccupancy}, {CurrentState.invln_numberofstoreys}, {invln_HomeType.Fields.invln_floorarea} is null calculation is not possible");
                return;
            }

            var concatenatevalue = $"{CurrentState.invln_numberofbedrooms}{CurrentState.invln_maxoccupancy}{CurrentState.invln_numberofstoreys}";
            var nddsStandard = _ndssRepository.GetByAttribute(invln_ndss.Fields.invln_StandardNumber, int.Parse(concatenatevalue), new string[] { invln_ndss.Fields.invln_StandardNumber, invln_ndss.Fields.invln_Standard }).FirstOrDefault();
            if (nddsStandard == null)
            {
                TracingService.Trace($"Cannot find entity {invln_ndss.EntityLogicalName} with value {concatenatevalue} in field {invln_ndss.Fields.invln_StandardNumber}");
                return; //throw new InvalidPluginExecutionException("Home type not covered by NDSS");
            }
            TracingService.Trace($"Calculate value in {invln_HomeType.Fields.invln_PercentageValueofNDSSStandard}");

            var pecentageValueOfNdssStandard = (decimal)(CurrentState.invln_floorarea / nddsStandard.invln_Standard.Value * 100);
            ExecutionData.Target.invln_PercentageValueofNDSSStandard = pecentageValueOfNdssStandard;
        }
    }
}
