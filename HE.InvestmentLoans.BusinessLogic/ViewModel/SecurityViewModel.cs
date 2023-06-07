using He.HelpToBuild.Apply.Application.Routing;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using System;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel
{
    public class SecurityViewModel 
    {
      
        public SecurityViewModel()
        {
            State = SecurityWorkflow.State.Index;
        }

        public string CheckAnswers { get; set; }
        public string ChargesDebt { get; set; }
        public string ChargesDebtCompany { get; set; }
        public string ChargesDebtCompanyInfo { get; set; }
        public string Debenture { get; set; }
        public string DirLoans { get; set; }
        public string DirLoansSub { get; set; }
        public string DirLoansSubMore { get; set; }
        public string LegalDevelopmentMore { get; set; }
        public string LegalDevelopment { get; set; }
        public string Name { get; set; }
        public SecurityWorkflow.State State { get;  set; }
        public bool StateChanged { get; set; }


        public void RemoveAlternativeRoutesData()
        {
            if(ChargesDebt == "No")
            {
                ChargesDebtCompanyInfo = null;
            }

            if (DirLoans == "No")
            {
                DirLoansSub = null;
                DirLoansSubMore = null;
            }
            else if(DirLoansSub == "Yes")
            {
                DirLoansSubMore = null;
            }
        }
    }
}
