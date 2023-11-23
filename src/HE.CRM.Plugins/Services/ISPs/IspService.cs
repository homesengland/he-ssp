using System;
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

        #endregion
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

        public void PopulateFieldsOnCreate(invln_ISP target)
        {
            if (target.invln_Loanapplication != null)
            {
                var loan = _loanApplicationRepository.GetById(target.invln_Loanapplication.Id);
                target.invln_ProjectName = loan.invln_ProjectName;
                target.invln_Submitter = loan.OwnerId.Name;
                target.invln_Region = loan.invln_Region;
                target.invln_SPPIMet = loan.invln_AssessedasSPPI;
                target.invln_securities = loan.invln_Securities;

                if (loan.invln_Account != null)
                {
                    var organisation = _accountRepository.GetById(loan.invln_Account.Id, new string[] {nameof(Account.invln_CurrentCRR).ToLower(), nameof(Account.invln_rating).ToLower() } );
                    target.invln_CRR = organisation.invln_CurrentCRR;
                    target.invln_KYCRating = organisation.invln_rating;
                }

                if (loan.invln_Contact != null)
                {
                    var contact = _contactRepository.GetById(loan.invln_Contact.Id, new string[] { nameof(Contact.FullName).ToLower(), });
                    target.invln_Name = contact.FullName;
                }

                var siteDetailsRelatedToLoan = _siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(target.invln_Loanapplication);
                if (siteDetailsRelatedToLoan.Any())
                {
                    var programme = string.Empty;
                    switch (siteDetailsRelatedToLoan.First().invln_Programme?.Value)
                    {
                        case (int)invln_Programme.LevellingUpHomeBuildFund:
                            programme = "Leveling Up Home Build Fund";
                            break;
                        default:
                            break;
                    }
                    target.invln_Programme = programme;
                }
            }
        }

        public void SetFieldsOnSentForApprovalChange(invln_ISP target)
        {
            if(target.invln_SendforApproval == true)
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
            if ((preImage == null && target.invln_SendforApproval == true) || (preImage != null && preImage.invln_SendforApproval != true && target.invln_SendforApproval == true))
            {
                var desReviews = _reviewApprovalRepository.GetReviewApprovalsForIsp(target.ToEntityReference(), invln_reviewerapproverset.DESReview);
                var desReviewsNumber = desReviews.Count() > 0 ? (desReviews.Count() + 1).ToString() : "";

                var hofReviews = _reviewApprovalRepository.GetReviewApprovalsForIsp(target.ToEntityReference(), invln_reviewerapproverset.HoFReview);
                var hofReviewsNumber = hofReviews.Count() > 0 ? (hofReviews.Count() + 1).ToString() : "";

                var desTeam = _teamRepositoryAdmin.GetTeamByName("DES Team");
                var hofTeam = _teamRepositoryAdmin.GetTeamByName("HoF Team");

                // Create DES record
                this._reviewApprovalRepository.Create(new invln_reviewapproval()
                {
                    invln_name = "DES Review of ISP " + desReviewsNumber,
                    invln_Description = "DES Review of ISP " + desReviewsNumber,
                    invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.DESReview),
                    OwnerId = desTeam.ToEntityReference(),
                    invln_ispid = target.ToEntityReference()
                });

                // Create Hof record
                this._reviewApprovalRepository.Create(new invln_reviewapproval()
                {
                    invln_name = "HoF Review of ISP " + hofReviewsNumber,
                    invln_Description = "HoF Review of ISP " + hofReviewsNumber,
                    invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFReview),
                    OwnerId = hofTeam.ToEntityReference(),
                    invln_ispid = target.ToEntityReference()
                });
            }
        }

        #endregion
    }
}
