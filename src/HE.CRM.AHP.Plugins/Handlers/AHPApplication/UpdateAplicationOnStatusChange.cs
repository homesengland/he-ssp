using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class UpdateAplicationOnStatusChange : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        private readonly IHomeTypeRepository _homeTypeRepository;

        public UpdateAplicationOnStatusChange(IHomeTypeRepository homeTypeRepository)
        {
            _homeTypeRepository = homeTypeRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace("UpdateAplicationOnStatusChange - Can Work");
            return ValueChanged(invln_scheme.Fields.StatusCode);
        }

        public override void DoWork()
        {
            TracingService.Trace("UpdateAplicationOnStatusChange - Do Work");
            if (CurrentState.StatusCode.Value == (int)invln_scheme_StatusCode.ReferredBackToApplicant)
            {
                if (CurrentState.invln_representationsandwarrantiesconfirmation != true)
                    return;
                ExecutionData.Target.invln_representationsandwarrantiesconfirmation = false;
            }
            if (CurrentState.StatusCode.Value == (int)invln_scheme_StatusCode.ApplicationSubmitted)
            {
                var hometypes = _homeTypeRepository.GetByAttribute(invln_HomeType.Fields.invln_application,
                    CurrentState.Id,
                    new string[] { invln_HomeType.Fields.invln_PercentageValueofNDSSStandard,
                        invln_HomeType.Fields.invln_prospectiverentasofmarketrent,
                        invln_HomeType.Fields.invln_FirstTrancheSalesReceipt    });
                if (hometypes == null || hometypes.Count == 0)
                    return;
                ClearDataBeforeCalculation();
                MapData(hometypes);
            }
        }

        private void MapData(IList<invln_HomeType> hometypes)
        {
            var percentageValueofNDSSStandardMax = hometypes.Max(x => x.invln_PercentageValueofNDSSStandard);
            var percentageValueofNDSSStandardMin = hometypes.Min(x => x.invln_PercentageValueofNDSSStandard);
            var homeTypeWithMaxValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMax);
            var homeTypeWithMinValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMin);
            ExecutionData.Target.invln_Maximumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMax;
            ExecutionData.Target.invln_Minimumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMin;
            if (CurrentState.invln_Tenure.Value == (int)invln_Tenure.Affordablerent ||
                CurrentState.invln_Tenure.Value == (int)invln_Tenure.Renttobuy)
            {
                ExecutionData.Target.invln_MaxRentasofMarketRentoftheHomeTypeson = homeTypeWithMaxValue.invln_prospectiverentasofmarketrent;
                ExecutionData.Target.invln_MinRentasofMarketRentoftheHomeTypeson = homeTypeWithMinValue.invln_prospectiverentasofmarketrent;
            }

            if (CurrentState.invln_Tenure.Value == (int)invln_Tenure.Sharedownership ||
                CurrentState.invln_Tenure.Value == (int)invln_Tenure.OPSO ||
                CurrentState.invln_Tenure.Value == (int)invln_Tenure.HOLD)
            {
                ExecutionData.Target.invln_MaxAssumedFirstTrancheSaleoftheHomesType = homeTypeWithMaxValue.invln_SharedOwnershipInitialSale;
                ExecutionData.Target.invln_MinAssumedFirstTrancheSaleoftheHomesType = homeTypeWithMinValue.invln_SharedOwnershipInitialSale;
                ExecutionData.Target.invln_MaxRentasofUnsoldEquityfortheHomeTypes = homeTypeWithMaxValue.invln_proposedrentasaofunsoldshare;
                ExecutionData.Target.invln_MinRentasofUnsoldEquityfortheHomeTypes = homeTypeWithMinValue.invln_proposedrentasaofunsoldshare;
            }
        }

        private void ClearDataBeforeCalculation()
        {
            ExecutionData.Target.invln_Maximumm2asofNDSSoftheHomeTypesonthis = null;
            ExecutionData.Target.invln_Minimumm2asofNDSSoftheHomeTypesonthis = null;
            ExecutionData.Target.invln_MaxRentasofMarketRentoftheHomeTypeson = null;
            ExecutionData.Target.invln_MinRentasofMarketRentoftheHomeTypeson = null;
            ExecutionData.Target.invln_MaxAssumedFirstTrancheSaleoftheHomesType = null;
            ExecutionData.Target.invln_MinAssumedFirstTrancheSaleoftheHomesType = null;
            ExecutionData.Target.invln_MaxRentasofUnsoldEquityfortheHomeTypes = null;
            ExecutionData.Target.invln_MinRentasofUnsoldEquityfortheHomeTypes = null;
        }
    }
}
