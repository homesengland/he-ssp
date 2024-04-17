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

        public UpdateAplicationOnStatusChange(IHomeTypeRepository _homeTypeRepository)
        {
            this._homeTypeRepository = _homeTypeRepository;
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
                var hometype = _homeTypeRepository.GetByAttribute(invln_HomeType.Fields.invln_application,
                    CurrentState.Id, new string[] { invln_HomeType.Fields.invln_PercentageValueofNDSSStandard });
                if (hometype == null || hometype.Count == 0)
                    return;
                var percentageValueofNDSSStandardMax = hometype.MaxBy(x => x.)
                //var percentageValueofNDSSStandardMin
            }

        }
    }
}