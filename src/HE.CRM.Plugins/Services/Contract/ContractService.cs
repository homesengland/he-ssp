using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.Contract
{
    public class ContractService : CrmService, IContractService
    {
        #region Fields
        private readonly ILoanApplicationRepository loanApplicationRepository;

        #endregion

        #region Constructors

        public ContractService(CrmServiceArgs args) : base(args)
        {
            loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
        }
        #endregion

        #region Public Methods

        public void RejectAddingWhenRelatedLoanInApprovedStatus(invln_contract target, invln_contract preImage)
        {
            if (target.invln_loanapplicationid != null || (preImage != null && preImage.invln_loanapplicationid != null))
            {
                var loan = loanApplicationRepository.GetById(target.invln_loanapplicationid?.Id ?? preImage.invln_loanapplicationid.Id);
                if (loan.StatusCode.Value != (int)invln_Loanapplication_StatusCode.ApprovedSubjectToContract)
                {
                    throw new InvalidPluginExecutionException("Cannot add contract for loan application when status is not Approved Subject To Contract");
                }
            }
        }

        #endregion
    }
}
