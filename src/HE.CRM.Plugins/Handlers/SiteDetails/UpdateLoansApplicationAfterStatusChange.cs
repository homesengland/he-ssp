using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.SiteDetails
{
    public class UpdateLoansApplicationAfterStatusChange : CrmEntityHandlerBase<invln_SiteDetails, DataverseContext>
    {
        private readonly ISiteDetailsRepository _siteDetailsRepository;

        private readonly ILoanApplicationRepository _loanApplicationRepository;

        public UpdateLoansApplicationAfterStatusChange(ISiteDetailsRepository siteDetailsRepository,
                                                        ILoanApplicationRepository loanApplicationRepository)
        {
            this._siteDetailsRepository = siteDetailsRepository;
            this._loanApplicationRepository = loanApplicationRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_SiteDetails.Fields.invln_completionstatus) && ExecutionData.IsMessage(CrmMessage.Update);
        }

        public override void DoWork()
        {
            var siteDetails = _siteDetailsRepository.GetSiteDetailRelatedToLoanApplication(CurrentState.invln_Loanapplication);
            if (siteDetails.Count == 1)
            {
                UpdateLoansApplication(CurrentState.invln_completionstatus.Value);
                return;
            }
            switch (CurrentState.invln_completionstatus.Value)
            {
                case (int)invln_Sectioncompletionstatus.Notstarted:
                {
                    UpdateLoansApplication((int)invln_Sectioncompletionstatus.Notstarted);

                    break;
                }
                case (int)invln_Sectioncompletionstatus.Inprogress:
                {
                    if (IsInErlierStatus(new List<int> { (int)invln_Sectioncompletionstatus.Notstarted }, siteDetails))
                        break;
                    UpdateLoansApplication((int)invln_Sectioncompletionstatus.Inprogress);

                    break;
                }
                case (int)invln_Sectioncompletionstatus.NotSubmitted:
                {
                    if (IsInErlierStatus(new List<int> { (int)invln_Sectioncompletionstatus.Notstarted,
                                                            (int)invln_Sectioncompletionstatus.Inprogress },
                                                            siteDetails))
                        break;

                    UpdateLoansApplication((int)invln_Sectioncompletionstatus.NotSubmitted);

                    break;
                }
                case (int)invln_Sectioncompletionstatus.Submitted:
                {
                    if (IsInErlierStatus(new List<int> { (int)invln_Sectioncompletionstatus.Notstarted,
                                                            (int)invln_Sectioncompletionstatus.Inprogress,
                                                            (int)invln_Sectioncompletionstatus.NotSubmitted},
                                                            siteDetails))
                        break;

                    UpdateLoansApplication((int)invln_Sectioncompletionstatus.Submitted);

                    break;
                }
                case (int)invln_Sectioncompletionstatus.Completed:
                {
                    if (IsInErlierStatus(new List<int> { (int)invln_Sectioncompletionstatus.Notstarted,
                                                            (int)invln_Sectioncompletionstatus.Inprogress,
                                                            (int)invln_Sectioncompletionstatus.NotSubmitted,
                                                            (int)invln_Sectioncompletionstatus.Submitted }
                                                            , siteDetails))
                        break;

                    UpdateLoansApplication((int)invln_Sectioncompletionstatus.Completed);
                    break;
                }
                default:
                    break;
            }
        }

        private bool IsInErlierStatus(List<int> listOfErlierStatuses, List<invln_SiteDetails> siteDetails)
        {
            foreach (var status in listOfErlierStatuses)
            {
                var isInStatus = siteDetails.Where(x => x.Id != CurrentState.Id).Any(x => x.invln_completionstatus.Value == status);
                if (isInStatus)
                {
                    UpdateLoansApplication(status);
                    return true;
                }
            }
            return false;
        }

        private void UpdateLoansApplication(int status)
        {
            var appToUpdate = new invln_Loanapplication()
            {
                Id = CurrentState.invln_Loanapplication.Id,
                invln_sitedetailscompletionstatus = new OptionSetValue(status),
            };
            _loanApplicationRepository.Update(appToUpdate);
        }
    }
}
