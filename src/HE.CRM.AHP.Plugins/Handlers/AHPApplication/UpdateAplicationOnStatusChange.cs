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
            return ValueChanged(invln_scheme.Fields.StatusCode);
        }

        public override void DoWork()
        {
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
                if (CurrentState.invln_Tenure.Value == (int)invln_Tenure.Affordablerent ||
                    CurrentState.invln_Tenure.Value == (int)invln_Tenure.Renttobuy)
                {

                    var percentageValueofNDSSStandardMax = hometypes.Max(x => x.invln_PercentageValueofNDSSStandard);
                    var percentageValueofNDSSStandardMin = hometypes.Min(x => x.invln_PercentageValueofNDSSStandard);

                    var homeTypeWithMaxValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMax);
                    var homeTypeWithMinValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMin);
                    ExecutionData.Target.invln_Maximumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMax;
                    ExecutionData.Target.invln_Minimumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMin;
                    ExecutionData.Target.invln_MaxRentasofMarketRentoftheHomeTypeson = homeTypeWithMaxValue.invln_prospectiverentasofmarketrent;
                    ExecutionData.Target.invln_MaxRentasofMarketRentoftheHomeTypeson = homeTypeWithMaxValue.invln_prospectiverentasofmarketrent;
                }

                if (CurrentState.invln_Tenure.Value == (int)invln_Tenure.Sharedownership ||
                    CurrentState.invln_Tenure.Value == (int)invln_Tenure.OPSO ||
                    CurrentState.invln_Tenure.Value == (int)invln_Tenure.HOLD)
                {
                    var percentageValueofNDSSStandardMax = hometypes.Max(x => x.invln_PercentageValueofNDSSStandard);
                    var percentageValueofNDSSStandardMin = hometypes.Min(x => x.invln_PercentageValueofNDSSStandard);

                    var homeTypeWithMaxValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMax);
                    var homeTypeWithMinValue = hometypes.FirstOrDefault(x => x.invln_PercentageValueofNDSSStandard == percentageValueofNDSSStandardMin);
                    ExecutionData.Target.invln_Maximumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMax;
                    ExecutionData.Target.invln_Minimumm2asofNDSSoftheHomeTypesonthis = percentageValueofNDSSStandardMin;
                    ExecutionData.Target.invln_MaxAssumedFirstTrancheSaleoftheHomesType = homeTypeWithMinValue.invln_FirstTrancheSalesReceipt?.Value;
                    ExecutionData.Target.invln_MinAssumedFirstTrancheSaleoftheHomesType = homeTypeWithMinValue.invln_FirstTrancheSalesReceipt?.Value;
                    ExecutionData.Target.invln_MaxRentasofUnsoldEquityfortheHomeTypes = homeTypeWithMinValue.invln_RentasofUnsoldShare;
                    ExecutionData.Target.invln_MinRentasofUnsoldEquityfortheHomeTypes = homeTypeWithMinValue.invln_RentasofUnsoldShare;
                }
            }
        }
    }
}
