using System;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Cryptography;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.ISPs
{
    public class IspService : CrmService, IIspService
    {
        #region Fields

        private readonly ILoanApplicationRepository _loanApplicationRepository;

        private readonly IAccountRepository _accountRepository;

        private readonly IConditionRepository _conditionRepository;

        private readonly IContactRepository _contactRepository;

        private readonly IProjectSpecificConditionRepository _projectSpecificConditionRepository;

        private readonly ISiteDetailsRepository _siteDetailsRepository;

        private readonly IIspRepository _ispRepository;

        private readonly IReviewApprovalRepository _reviewApprovalRepository;

        private readonly ITeamRepository _teamRepositoryAdmin;

        #endregion Fields

        #region Constructors

        public IspService(CrmServiceArgs args) : base(args)
        {
            _loanApplicationRepository = CrmRepositoriesFactory.Get<ILoanApplicationRepository>();
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
            _conditionRepository = CrmRepositoriesFactory.Get<IConditionRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _projectSpecificConditionRepository = CrmRepositoriesFactory.Get<IProjectSpecificConditionRepository>();
            _siteDetailsRepository = CrmRepositoriesFactory.Get<ISiteDetailsRepository>();
            _ispRepository = CrmRepositoriesFactory.Get<IIspRepository>();
            _reviewApprovalRepository = CrmRepositoriesFactory.Get<IReviewApprovalRepository>();

            _teamRepositoryAdmin = CrmRepositoriesFactory.GetSystem<ITeamRepository>();
        }

        public void SetFieldsOnSentForApprovalChange(invln_ISP target)
        {
            if (target.invln_SendforApproval == true)
            {
                target.invln_DateSubmitted = DateTime.UtcNow;
                target.invln_DateSentforApproval = DateTime.UtcNow;
            }
        }

        public void CreateProjectConditionRecordsForIsp(invln_ISP target)
        {
            if (target.invln_Loanapplication != null)
            {
                var conditions = _conditionRepository.GetBespokeConditionsForLoanApplication(target.invln_Loanapplication.Id);
                foreach (var condition in conditions)
                {
                    var conditionToCreate = new invln_ProjectSpecificCondition()
                    {
                        invln_Description = condition.invln_ConditionDefinition,
                        invln_ISP = target.ToEntityReference(),
                    };
                    _projectSpecificConditionRepository.Create(conditionToCreate);
                }
            }
        }

        public void CreateDesAndHofRecords(invln_ISP preImage, invln_ISP target)
        {

        }

        #endregion Constructors
    }
}
