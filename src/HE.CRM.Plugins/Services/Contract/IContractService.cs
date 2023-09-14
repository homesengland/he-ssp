using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.Contract
{
    public interface IContractService : ICrmService
    {
        void RejectAddingWhenRelatedLoanInApprovedStatus(invln_contract target, invln_contract preImage);
    }
}
